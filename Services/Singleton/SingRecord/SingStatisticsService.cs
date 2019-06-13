using Services.EF;
using Services.Singleton.ISingRecord;
using System.Collections.Generic;

namespace Services.Singleton.SingRecord
{
    /// <summary>
    /// 统计模块数据层
    /// </summary>
    public sealed class SingStatisticsService : BaseService, ISingStatistics
    {
        #region 单例模式

        private static SingStatisticsService _singStatisticsService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static SingStatisticsService Instance
        {
            get
            {
                if (_singStatisticsService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_singStatisticsService == null)
                        {
                            _singStatisticsService = new SingStatisticsService();
                        }
                    }
                }
                return _singStatisticsService;
            }
        }

        #endregion 单例模式

        #region Public Methods

        public IList<ClickCountReportEntity> GetClickCountReport()
        {
            return ExecuteSqlQueryReturnObjects<ClickCountReportEntity>("exec proc_ClickCountReport");
        }

        #endregion Public Methods
    }
}