using System;

namespace TaobaoArea.EntityFramework.Entities
{
    public class TaobaoRegion
    {
        public int Id { get; set; }

        public string ParentCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public string ZipCode { get; set; }

        public int Level { get; set; }

        public bool IsDel { get; set; }

        public DateTime CreationTime { get; set; }

        public int Status { get; set; }

        public TaobaoRegion()
        {
            Status = 0;
            IsDel = false;
            CreationTime = DateTime.Now;
        }
    }
}
