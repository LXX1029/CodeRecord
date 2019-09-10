﻿using DataEntitys;
using Services.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Unity
{
    public interface IUnityStatisticsFacade : IRepository<ClickCountReportEntity>
    {
        #region Public Methods
        Task<List<ClickCountReportEntity>> GetClickCountReport();
        #endregion Public Methods
    }
}