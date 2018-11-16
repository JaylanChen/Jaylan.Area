using System.Collections.Generic;

namespace Mi.Area.Models
{
    public class TownResultModel
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Msg { get; set; }

        public RegionModel Data { get; set; }
    }

    public class RegionModel
    {
        public Dictionary<string, TownModel> Regions { get; set; }
    }

}
