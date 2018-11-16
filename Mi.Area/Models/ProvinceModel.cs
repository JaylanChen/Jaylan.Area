using System.Collections.Generic;

namespace Mi.Area.Models
{
    public class ProvinceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CityModel> Child { get; set; }
    }
}
