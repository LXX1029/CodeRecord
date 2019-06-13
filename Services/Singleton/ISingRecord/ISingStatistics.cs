using Services.EF;
using System.Collections.Generic;

namespace Services.Singleton.ISingRecord
{
    internal interface ISingStatistics
    {
        #region Public Methods

        IList<ClickCountReportEntity> GetClickCountReport();

        #endregion Public Methods
    }
}