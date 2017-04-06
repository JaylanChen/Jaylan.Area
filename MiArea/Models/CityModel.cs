using System.Collections.Generic;

namespace MiArea.Models
{
    public class CityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CountyModel> Child { get; set; }
    }
}
