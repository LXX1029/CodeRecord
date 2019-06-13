using Services.EF;
using System.Collections.Generic;

namespace Services.Singleton.ISingRecord
{
    internal interface ISingUser
    {
        #region Public Methods

        bool AddUser(DevelopUser user);
        bool DeleteUser(DevelopUser user);
        bool DeleteUserByUserId(int userId);
        bool UpdateUser(DevelopUser user);
        DevelopUser GetDevelopUser(string name, string pwd);
        DevelopUser GetDevelopUserByUserId(int userId);
        DevelopUser GetDevelopUserByUserIdAsync(int userId);
        IList<DevelopUser> GetDevelopUsers();

        #endregion Public Methods
    }
}