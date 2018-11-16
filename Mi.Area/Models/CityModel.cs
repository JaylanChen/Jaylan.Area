using System.Collections.Generic;

namespace Mi.Area.Models
{
    public class CityModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CountyModel> Child { get; set; }
    }
}
