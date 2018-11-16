using HtmlAgilityPack;
using Jaylan.Area.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NBS.Area
{

    //详细地区
    public static class NBSAreaHelper
    {
        /// <summary>
        /// 获取行政区域
        /// </summary>
        /// <param name="areaBaseUrl"></param>
        public static void GetAllAreas(string areaBaseUrl)
        {
            GetProvince(areaBaseUrl);
        }

        /// <summary>
        /// 省市
        /// </summary>
        private static void GetProvince(string areaBaseUrl)
        {
            Console.WriteLine("省级获取开始");
            //HtmlWeb htmlWeb = new HtmlWeb();
            //htmlWeb.OverrideEncoding = Encoding.GetEncoding("GB2312");
            //var doc = htmlWeb.Load(areaBaseUrl);

            var htmlContent = WebHtmlHelper.GetHtmlContent(areaBaseUrl);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var provinceTrs = htmlDoc.DocumentNode.SelectNodes("//tr[@class='provincetr']");
            var areaList = new List<NBS_Area>();
            foreach (var provinceTr in provinceTrs)
            {
                var provinceTdNodes = provinceTr.SelectNodes("td");
                foreach (var provinceTd in provinceTdNodes)
                {
                    var aNodes = provinceTd.SelectNodes("a");
                    if (aNodes == null || aNodes.Count == 0)
                    {
                        continue;
                    }
                    var provinceNode = aNodes.First();
                    var href = provinceNode.Attributes["href"].Value;
                    var code = GetCodeByHref(href);
                    if (string.IsNullOrEmpty(code))
                    {
                        continue;
                    }
                    var name = provinceNode.InnerText;

                    var region = new NBS_Area
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
            areaList.Add(new NBS_Area
            {
                Name = "台湾省",
                Code = "71",
                ParentCode = string.Empty,
                Level = 1,
                Status = 1//使用
            });
            areaList.Add(new NBS_Area
            {
                Name = "香港特别行政区",
                Code = "81",
                ParentCode = string.Empty,
                Level = 1,
                Status = 1//使用
            });
            areaList.Add(new NBS_Area
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
                    if (dbContext.NBS_Area.Any(a => a.Code == area.Code))
                    {
                        Console.WriteLine($"Code:{area.Code},Name:{area.Name} 已存在");
                        continue;
                    }
                    dbContext.NBS_Area.Add(area);
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
        /// <param name="parentAreaList"></param>
        private static void GetCitys(List<NBS_Area> parentAreaList)
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
        /// <param name="parentAreaList"></param>
        private static void GetCountys(List<NBS_Area> parentAreaList)
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
        /// <param name="parentAreaList"></param>
        private static void GetTowns(List<NBS_Area> parentAreaList)
        {
            if (!parentAreaList.Any())
            {
                return;
            }
            Console.WriteLine("乡镇/街道级获取开始：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var currentAreaList = GetAreas(parentAreaList, "towntr");
            Console.WriteLine("乡镇/街道级获取结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        }


        /// <summary>
        /// 获取子级区域
        /// </summary>
        /// <param name="parentAreaList"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        private static List<NBS_Area> GetAreas(List<NBS_Area> parentAreaList, string className)
        {
            if (!parentAreaList.Any())
            {
                return new List<NBS_Area>();
            }
            var random = new Random();
            var areaList = new List<NBS_Area>();
            var addedCount = 0;
            foreach (var parentArea in parentAreaList)
            {
                if (string.IsNullOrEmpty(parentArea.ChildNodeUrl))
                {
                    continue;
                }

                var htmlContent = WebHtmlHelper.GetHtmlContent(parentArea.ChildNodeUrl);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var areaTrs = htmlDoc.DocumentNode.SelectNodes("//tr[@class='" + className + "']");
                var tempAreaList = new List<NBS_Area>();
                if (areaTrs == null)
                {
                    var currentRegion = new NBS_Area
                    {
                        ParentCode = parentArea.Code,
                        Level = parentArea.Level + 1,
                        Name = parentArea.Name,
                        Code = parentArea.Code + "00",
                        ChildNodeUrl = parentArea.ChildNodeUrl,
                        Status = 3 //没有改级区域，手动维护的
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
                        var areaTds = areaTr.SelectNodes("td");
                        var areas = areaTds[1].SelectNodes("a");
                        var currentRegion = new NBS_Area
                        {
                            ParentCode = parentArea.Code,
                            Level = parentArea.Level + 1
                        };
                        var name = areaTds[1].InnerText.Trim();
                        if (areas == null)
                        {
                            var aCode = areaTds[0].InnerText.TrimEnd('0');
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
                        if (dbContext.NBS_Area.Any(a => a.Code == area.Code))
                        {
                            Console.WriteLine($"Code:{area.Code},Name:{area.Name} 已存在");
                            continue;
                        }
                        dbContext.NBS_Area.Add(area);
                        addedCount++;
                    }

                    parentArea.IsGetChild = true;
                    dbContext.NBS_Area.Update(parentArea);
                    dbContext.SaveChanges();
                }
                areaList.AddRange(tempAreaList);
                var sleepTime = random.Next(500, 800) + parentArea.Level * 60;
                Thread.Sleep(sleepTime);
            }
            Console.WriteLine($"共 {areaList.Count}条,添加 {addedCount}条");
            return areaList;
        }


        /// <summary>
        /// 获取完整URL路径
        /// </summary>
        /// <param name="currentUrl"></param>
        /// <param name="hrefUrl"></param>
        /// <returns></returns>
        private static string GetHrefFullUrl(string currentUrl, string hrefUrl)
        {
            var last = currentUrl.LastIndexOf("/", StringComparison.OrdinalIgnoreCase);
            var baseUrl = currentUrl.Substring(0, last + 1);
            return baseUrl + hrefUrl;
        }

        /// <summary>
        /// 根据URL获取区域编码
        /// </summary>
        /// <param name="hrefUrl"></param>
        /// <returns></returns>
        private static string GetCodeByHref(string hrefUrl)
        {
            if (string.IsNullOrEmpty(hrefUrl))
            {
                return null;
            }
            var slashIndex = hrefUrl.LastIndexOf("/", StringComparison.OrdinalIgnoreCase);
            if (slashIndex > 0)
            {
                hrefUrl = hrefUrl.Substring(slashIndex + 1);
            }
            var dotIndex = hrefUrl.IndexOf(".", StringComparison.OrdinalIgnoreCase);
            if (dotIndex < 0)
            {
                return hrefUrl;
            }
            var code = hrefUrl.Substring(0, dotIndex);
            return code;
        }

    }
}
