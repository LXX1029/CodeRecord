using Services.EF;
using System.Collections.Generic;

namespace Services.Singleton.ISingRecord
{
    internal interface ISingDevelopType
    {
        #region Public Methods

        bool AddDevelopType(DevelopType type);
        bool DeleteDeveloptype(DevelopType type);
        bool UpdateDeveloptype(DevelopType type);
        DevelopType GetDevelopTypeById(int id);
        IList<DevelopType> GetDevelopTypeList();
        IList<DevelopType> GetDevelopTypeListByFilter(string name, int parentId);
        IList<DevelopType> GetDevelopTypesByParentId(int parentId);
        int GetMaxDevelopTypeId();

        #endregion Public Methods
    }
}