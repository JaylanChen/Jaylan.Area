namespace Jaylan.Area.Data
{
    /// <summary>
    /// National Bureau Statistics
    /// </summary>
    public class NBS_Area : AreaBase
    {
        /// <summary>
        /// 是否以获取子区域
        /// </summary>
        public bool IsGetChild { get; set; }

        /// <summary>
        /// 子级地区URL
        /// </summary>
        public string ChildNodeUrl { get; set; }

        /// <summary>
        /// 状态
        /// 1 正常可用
        /// 2 修改名称，直辖市 的二级区域从 市辖区 改为 父级名称
        /// </summary>
        public int Status { get; set; }
    }
}
