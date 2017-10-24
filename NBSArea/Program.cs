using System;

namespace NBSArea
{
    class Program
    {
        static void Main(string[] args)
        {
            NBSAreaHelper.GetAllRegions();

            //const string areaUrl = "http://www.stats.gov.cn/tjsj/tjbz/xzqhdm/201703/t20170310_1471429.html";
            //NBSAreaHelper.GetAllAreas(areaUrl);
            Console.WriteLine("All Done");
            Console.ReadLine();
        }
    }
}
