namespace Jaylan.Area.Data
{
    /// <summary>
    /// MI Area
    /// </summary>
    public class Mi_Area : AreaBase
    {
        /// <summary>
        /// Parent Mi Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Mi Id
        /// </summary>
        public int MiId { get; set; }
        
        public int Status { get; set; }
    }
}
