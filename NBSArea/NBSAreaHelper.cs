using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using NBSArea.EntityFramework;
using SufeiUtil;

namespace NBSArea
{
    //详细地区
    public static class NBSAreaHelper
    {

        public static void GetAllRegions()
        {
            GetProvince();

        }

        /// <summary>
        /// 省市
        /// </summary>
        private static void GetProvince()
        {
            Console.WriteLine("省级获取开始");
            const string areaBaseUrl = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2016/index.html";
            var item = new HttpItem
            {
                URL = areaBaseUrl
            };
            var htmlDoc = new HtmlDocument();
            var http = new HttpHelper();
            var result = http.GetHtml(item);
            htmlDoc.LoadHtml(result.Html);
            var provincetrs = htmlDoc.DocumentNode.SelectNodes("//tr[@class='provincetr']");
            var areaList = new List<EntityFramework.Entities.NBSArea>();
            foreach (var provincetr in provincetrs)
            {
                var provinceTdNodes = provincetr.SelectNodes("td");
                foreach (var provinceTd in provinceTdNodes)
                {
                    var provincea = provinceTd.SelectNodes("a")[0];
                    var href = provincea.Attributes["href"].Value;
                    var name = provincea.InnerText;
                    var code = GetCodeByHref(href);
                    if (string.IsNullOrEmpty(code))
                    {
                        continue;
                    }

                    var region = new EntityFramework.Entities.NBSArea
                    {
                        Name = name.Trim(),
                        Code = code,
                        ParentCode = string.Empty,
                        Level = 1,
                        ChildNodeUrl = GetHrefFullUrl(areaBaseUrl, href),
                        Status = 1//使用
                    };
                    areaList.Add(region);
                }
            }
            //710000     台湾省
            //810000     香港特别行政区
            //820000     澳门特别行政区
            areaList.Add(new EntityFramework.Entities.NBSArea
            {
                Name = "台湾省",
                Code = "71",
                ParentCode = string.Empty,
                Level = 1,
                Status = 1//使用
            });
            areaList.Add(new EntityFramework.Entities.NBSArea
            {
                Name = "香港特别行政区",
                Code = "81",
                ParentCode = string.Empty,
                Level = 1,
                Status = 1//使用
            });
            areaList.Add(new EntityFramework.Entities.NBSArea
            {
                Name = "澳门特别行政区",
                Code = "82",
                ParentCode = string.Empty,
                Level = 1,
                Status = 1//使用
            });
            var i = 0;
            using (var dbContext = new AreaDbContext())
            {
                areaList = areaList.OrderBy(r => r.Code).ToList();
                foreach (var area in areaList)
                {
                    if (dbContext.NBSArea.Any(a => a.Code == area.Code))
                    {
                        Console.WriteLine($"Code:{area.Code},Name:{area.Name} 已存在");
                        continue;
                    }
                    dbContext.NBSArea.Add(area);
                    i++;
                }
                dbContext.SaveChanges();
            }
            Console.WriteLine($"省级获取完毕:共 {areaList.Count}条,添加 {i}条");
            GetCitys(areaList);
        }


        /// <summary>
        /// 市
        /// </summary>
        /// <param name="areaUrlList"></param>
        private static void GetCitys(List<EntityFramework.Entities.NBSArea> parentAreaList)
        {
            if (!parentAreaList.Any())
            {
                return;
            }
            Console.WriteLine("市级获取开始：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var currentAreaList = GetAreas(parentAreaList, "citytr");
            Console.WriteLine("市级获取结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            GetCountys(currentAreaList);
        }

        /// <summary>
        /// 区县
        /// </summary>
        /// <param name="areaUrlList"></param>
        private static void GetCountys(List<EntityFramework.Entities.NBSArea> parentAreaList)
        {
            if (!parentAreaList.Any())
            {
                return;
            }
            Console.WriteLine("区县级获取开始：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var currentAreaList = GetAreas(parentAreaList, "countytr");

            Console.WriteLine("区县级获取结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            GetTowns(currentAreaList);
        }

        /// <summary>
        /// 乡镇/街道
        /// </summary>
        /// <param name="areaUrlList"></param>
        private static void GetTowns(List<EntityFramework.Entities.NBSArea> parentAreaList)
        {
            if (!parentAreaList.Any())
            {
                return;
            }
            Console.WriteLine("乡镇/街道级获取开始：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var currentAreaList = GetAreas(parentAreaList, "towntr");
            Console.WriteLine("乡镇/街道级获取结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        }


        private static List<EntityFramework.Entities.NBSArea> GetAreas(List<EntityFramework.Entities.NBSArea> parentAreaList, string className)
        {
            if (!parentAreaList.Any())
            {
                return new List<EntityFramework.Entities.NBSArea>();
            }
            var item = new HttpItem();
            var htmlDoc = new HtmlDocument();
            var http = new HttpHelper();

            var random = new Random();
            var areaList = new List<EntityFramework.Entities.NBSArea>();
            var addedCount = 0;
            foreach (var parentArea in parentAreaList)
            {
                if (string.IsNullOrEmpty(parentArea.ChildNodeUrl))
                {
                    continue;
                }
                item.URL = parentArea.ChildNodeUrl;
                var result = http.GetHtml(item);
                var times = 5;
                while (result.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm ") + result.StatusCode + result.StatusDescription + " 获取过于频繁，休息 " + times + "分钟后重新开始获取");
                    Thread.Sleep(times * 1000);
                    result = http.GetHtml(item);
                    times++;
                }
                htmlDoc.LoadHtml(result.Html);
                var areaTrs = htmlDoc.DocumentNode.SelectNodes("//tr[@class='" + className + "']");
                var tempAreaList = new List<EntityFramework.Entities.NBSArea>();
                if (areaTrs == null)
                {
                    var currentRegion = new EntityFramework.Entities.NBSArea
                    {
                        ParentCode = parentArea.Code,
                        Level = parentArea.Level + 1,
                        Name = parentArea.Name,
                        Code = parentArea.Code + "00",
                        ChildNodeUrl = parentArea.ChildNodeUrl,
                        Status = 3 //没有改级，手动维护的
                    };
                    if (tempAreaList.Any(a => a.Code == currentRegion.Code))
                    {
                        continue;
                    }
                    tempAreaList.Add(currentRegion);

                }
                else
                {
                    foreach (var areaTr in areaTrs)
                    {
                        var areatds = areaTr.SelectNodes("td");
                        var areas = areatds[1].SelectNodes("a");
                        var currentRegion = new EntityFramework.Entities.NBSArea
                        {
                            ParentCode = parentArea.Code,
                            Level = parentArea.Level + 1
                        };
                        var name = areatds[1].InnerText.Trim();
                        if (areas == null)
                        {
                            var aCode = areatds[0].InnerText.TrimEnd('0');
                            if (aCode.Length % 2 != 0)
                            {
                                aCode += "0";
                            }
                            currentRegion.Name = name;
                            currentRegion.Code = aCode;
                            currentRegion.Status = -1;//不使用
                            tempAreaList.Add(currentRegion);
                            continue;
                        }

                        var area = areas[0];
                        name = area.InnerText.Trim();
                        if (name == "市辖区")
                        {
                            name = parentArea.Name;
                        }
                        var href = area.Attributes["href"].Value;
                        var code = GetCodeByHref(href);

                        currentRegion.Name = name;
                        currentRegion.Code = code;
                        currentRegion.ChildNodeUrl = GetHrefFullUrl(parentArea.ChildNodeUrl, href);
                        currentRegion.Status = 2;//改名了，直辖市 的二级区域 从市辖区 改为父级名称
                        if (tempAreaList.Any(a => a.Code == currentRegion.Code))
                        {
                            continue;
                        }
                        tempAreaList.Add(currentRegion);
                    }
                }

                using (var dbContext = new AreaDbContext())
                {
                    tempAreaList = tempAreaList.OrderBy(r => r.Code).ToList();
                    foreach (var area in tempAreaList)
                    {
                        if (dbContext.NBSArea.Any(a => a.Code == area.Code))
                        {
                            Console.WriteLine($"Code:{area.Code},Name:{area.Name} 已存在");
                            continue;
                        }
                        dbContext.NBSArea.Add(area);
                        addedCount++;
                    }

                    parentArea.IsGetChild = true;
                    dbContext.NBSArea.AddOrUpdate(parentArea);
                    dbContext.SaveChanges();
                }
                areaList.AddRange(tempAreaList);
                var sleepTime = random.Next(600, 1000) + parentArea.Level * 50;
                Thread.Sleep(sleepTime);
            }
            Console.WriteLine($"共 {areaList.Count}条,添加 {addedCount}条");
            return areaList;
        }



        private static string GetHrefFullUrl(string currentUrl, string hrefUrl)
        {
            var last = currentUrl.LastIndexOf("/");
            var baseUrl = currentUrl.Substring(0, last + 1);
            return baseUrl + hrefUrl;
        }

        private static string GetCodeByHref(string hrefUrl)
        {
            if (string.IsNullOrEmpty(hrefUrl))
            {
                return null;
            }
            var slashIndex = hrefUrl.LastIndexOf("/");
            if (slashIndex > 0)
            {
                hrefUrl = hrefUrl.Substring(slashIndex + 1);
            }
            var dotIndex = hrefUrl.IndexOf(".");
            if (dotIndex < 0)
            {
                return hrefUrl;
            }
            var code = hrefUrl.Substring(0, dotIndex);
            return code;
        }

    }
}
