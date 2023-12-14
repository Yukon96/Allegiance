using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    //fix transfers balance between account
    public interface IUserDao
    {
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User CreateUser(string username, string password);
        IList<User> GetUsers();
        Account GetBalanceByUsername(string username);
        public void ChangeBalance(decimal amount, int userId);
        public Transfer CreateTransfer(Transfer newtransfer);

        public List<Transfer> TransferLog(int id);
        public Account GetAccountByUserId(int id);
        public Transfer GetTransferByTransferId(int id);
        public User GetUserByAccountId(int id);
        public List<Transfer> GetPendingRequests(int account_id);
        public Transfer UpdateTransferStatus(int transferId, int transferStatus);
    }
}
