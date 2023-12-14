using Capstone.DAO;
using Capstone.Models;

namespace Test.TestDAO
{
    [TestClass]
    public class UserDaoTests : BaseDaoTests
    {
        UserSqlDao dao;
        [TestInitialize]
        public virtual void Init()
        {
            dao = new UserSqlDao(connectionString);
        }

        [TestMethod]
        public void GetUsersTest()
        {
            IList<ReturnUser> users = dao.GetUsers();
            Assert.AreEqual(4, users.Count);
        }

        [TestMethod]
        public void GetUserByIdHappyPath()
        {
            User user = dao.GetUserById(1);
            Assert.AreEqual("user", user.Username);
        }

        [TestMethod]
        public void GetUserByIdInvalidId()
        {
            User user = dao.GetUserById(12);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetFullUserByUsernameHappyPath()
        {
            User user = dao.GetFullUserByUsername("user");
            Assert.AreEqual(1, user.UserId);
        }


        [TestMethod]
        public void GetFullUserByUsernameInvalidUsername()
        {
            User user = dao.GetFullUserByUsername("invalid");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetFullUserByEmailHappyPath()
        {
            User user = dao.GetFullUserByEmail("dummy@email.net");
            Assert.AreEqual(1, user.UserId);
        }

        [TestMethod]
        public void GetFullUserByEmailInvalidEmail()
        {
            User user = dao.GetFullUserByEmail("notanemail");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByUsernameHappyPath()
        {
            ReturnUser user = dao.GetUserByUsername("user");
            Assert.AreEqual(1, user.UserId);
        }

        [TestMethod]
        public void GetUserByUsernameInvalidUsername()
        {
            ReturnUser user = dao.GetUserByUsername("invalid");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void CreateUserAddsUser()
        {
            dao.CreateUser("username", "email", "org", "password", "user");
            IList<ReturnUser> users = dao.GetUsers();
            Assert.AreEqual(5, users.Count);
        }

        [TestMethod]
        public void CreateUserCreatedUserNotNull()
        {
            dao.CreateUser("username", "email", "org", "password", "user");
            User user = dao.GetFullUserByUsername("username");
            Assert.IsNotNull(user.Username);
        }

        [TestMethod]
        public void ChangePasswordHashChanged()
        {
            dao.ChangePassword("user", "newPassword");
            User user = dao.GetFullUserByUsername("user");
            Assert.AreNotEqual("Jg45HuwT7PZkfuKTz6IB90CtWY4", user.PasswordHash);
        }

        [TestMethod]
        public void ChangePasswordSaltChanged()
        {
            dao.ChangePassword("user", "newpassword");
            User user = dao.GetFullUserByUsername("user");
            Assert.AreNotEqual("lhxp4xh7bn0", user.Salt);
        }

        [TestMethod]
        public void ChangePasswordOTPBoolFalse()
        {
            dao.ChangePassword("user", "newpassword");
            User user = dao.GetFullUserByUsername("user");
            Assert.AreEqual(false, user.HasOneTimePassword);
        }

        [TestMethod]
        public void GenerateOneTimePasswordOTPBoolTrue()
        {
            dao.GenerateOneTimePassword("user");
            User user = dao.GetFullUserByUsername("user");
            Assert.AreEqual(true, user.HasOneTimePassword);
        }
        [TestMethod]
        public void GenerateOneTimePasswordHashChanged()
        {
            dao.GenerateOneTimePassword("user");
            User user = dao.GetFullUserByUsername("user");
            Assert.AreNotEqual("Jg45HuwT7PZkfuKTz6IB90CtWY4=", user.PasswordHash);
        }

        [TestMethod]
        public void GenerateOneTimePasswordSaltChanged()
        {
            dao.GenerateOneTimePassword("user");
            User user = dao.GetFullUserByUsername("user");
            Assert.AreNotEqual("lhxp4xh7bn0", user.Salt);
        }
    }
}