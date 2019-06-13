using Services.EF;
using System.Collections.Generic;

namespace Services.Singleton.ISingRecord
{
    internal interface ISingDevelopFun
    {
        #region Public Methods

        IList<DevelopFun> GetDevelopFuns();

        #endregion Public Methods
    }
}