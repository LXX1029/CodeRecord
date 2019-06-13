using Services.EF;
using System.Collections.Generic;

namespace Services.Singleton.ISingRecord
{
    internal interface ISingDevelopPowerFun
    {
        #region Public Methods

        IList<DevelopPowerFun> GetDevelopPowerFunsByUserId(int userId);
        IList<View_DevelopUserPowerFun> GetViewDevelopUserPowerFunByUserId(int userId);
        bool SetDevelopPowerFun(int userId, int funId, bool isEnabled);

        #endregion Public Methods
    }
}