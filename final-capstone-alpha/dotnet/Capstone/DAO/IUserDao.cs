using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.DAO
{
    public interface IUserDao
    {
        IList<ReturnUser> GetUsers();
        User GetUserById(int id);
        ReturnUser GetUserByUsername(string username);
        User GetFullUserByUsername(string username);
        User GetFullUserByEmail(string email);
        User CreateUser(string username, string email, string organizationName, string password, string role);
        void ChangePassword(string username, string password);
        string GenerateOneTimePassword(string username);

        //TODO (Neil) delete user method
    }
}
