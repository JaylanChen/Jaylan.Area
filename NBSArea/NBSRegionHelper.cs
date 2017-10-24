using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NBSArea.EntityFramework;
using NBSArea.EntityFramework.Entities;

namespace NBSArea
{
    /// <summary>
    /// National Bureau of Standards
    /// </summary>
    public static class NBSRegionHelper
    {
        public static void GetAllAreas(string nbsAreaUrl)
        {
            if (string.IsNullOrEmpty(nbsAreaUrl))
            {
                Console.WriteLine("省市区地址不能为空");
                return;
            }

            var webClient = new HtmlWeb();
            var doc = webClient.Load(nbsAreaUrl);
            var nodes = doc.DocumentNode.SelectNodes("//p[@class='MsoNormal']");//选取所有地区数据的p标签
            var parentRegions = new NBSRegion[4];
            var regionList = new List<NBSRegion>();
            foreach (var currentNode in nodes)
            {
                HtmlNode codeNode;
                HtmlNode nameNode;
                if (currentNode.SelectNodes("b") != null)
                {
                    if (currentNode.SelectNodes("b").Count == 1)
                    {
                        codeNode = currentNode.SelectSingleNode("span[1]");
                        nameNode = currentNode.SelectSingleNode("b[1]");
                    }
                    else
                    {
                        codeNode = currentNode.SelectSingleNode("b[1]");
                        nameNode = currentNode.SelectSingleNode("b[2]");
                    }
                }
                else
                {
                    codeNode = currentNode.SelectNodes("span")[1];
                    nameNode = currentNode.SelectNodes("span")[2];
                }
                var code = codeNode.InnerText.Replace("&nbsp;", "").Trim();//对地区代码进行处理
                var name = nameNode.InnerText;//获取地区名字
                var realName = name.Replace("　", "");//处理地区名字占位符
                var level = name.Length - realName.Length + 1;//计算占位符个数 即 地区级别
                var region = new NBSRegion
                {
                    Level = level,
                    ParentCode = level > 1 ? parentRegions[level - 1].Code : "000000",
                    Code = code,
                    Name = realName == "市辖区" ? parentRegions[level - 1].Name : realName
                };
                parentRegions[level] = region;
                regionList.Add(region);
            }
            using (var dbContext = new AreaDbContext())
            {
                regionList = regionList.OrderBy(r => r.Code).ToList();
                dbContext.NBSRegion.AddRange(regionList);
                dbContext.SaveChanges();
            }

        }
    }
}
