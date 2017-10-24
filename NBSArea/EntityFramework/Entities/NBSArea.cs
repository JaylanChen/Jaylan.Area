using System;

namespace NBSArea.EntityFramework.Entities
{
    public class NBSArea
    {
        public int Id { get; set; }

        public string ParentCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public bool IsDel { get; set; }

        public DateTime CreationTime { get; set; }

        public int Status { get; set; }


        /// <summary>
        /// 子级地区URL
        /// </summary>
        public bool IsGetChild { get; set; }

        /// <summary>
        /// 子级地区URL
        /// </summary>
        public string ChildNodeUrl { get; set; }

        public NBSArea()
        {
            IsGetChild = false;
            ChildNodeUrl = string.Empty;
            Status = 0;
            IsDel = false;
            CreationTime = DateTime.Now;
        }
    }
}
