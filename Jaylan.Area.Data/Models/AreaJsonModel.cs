using System.Collections.Generic;

namespace Jaylan.Area.Data.Models
{
    public class AreaJsonModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 子区域
        /// </summary>
        public List<AreaJsonModel> Children { get; set; } = new List<AreaJsonModel>();
    }
}
