namespace DataEntitys
{
    /// <summary>
    /// 查看次数实体
    /// </summary>
    public sealed class ClickCountReportEntity
    {
        /// <summary>
        /// 小版块类型Id
        /// </summary>
        public int SubTypeId { get; set; }

        /// <summary>
        /// 小版块类型名称
        /// </summary>
        public string SubTypeName { get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        public int ClickCount { get; set; }
        /// <summary>
        /// 大版块类型Id
        /// </summary>
        public int ParentTypeId { get; set; }

        /// <summary>
        /// 大版块类型名称
        /// </summary>
        public string ParentTypeName { get; set; }
    }
}