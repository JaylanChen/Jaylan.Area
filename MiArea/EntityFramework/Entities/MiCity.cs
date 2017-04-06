using System;

namespace MiArea.EntityFramework.Entities
{
    public class MiCity
    {
        public int Id { get; set; }

        public int MiId { get; set; }

        public int ParentId { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string ZipCode { get; set; }

        public int Status { get; set; }

        public bool IsDel { get; set; }
        public DateTime CreationTime { get; set; }

        public MiCity()
        {
            IsDel = false;
            CreationTime = DateTime.Now;
        }
    }
}
