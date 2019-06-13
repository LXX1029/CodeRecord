using Services.EF;

namespace Services.MEF.IRecord
{
    public interface IUserService
    {
        bool AddUser(DevelopUser user);

        DevelopUser GetDevelopUser(string name, string pwd);
    }
}