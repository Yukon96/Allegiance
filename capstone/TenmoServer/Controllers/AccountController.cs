using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserDao userDao;
        public AccountController(IUserDao userDao)
        {
            this.userDao = userDao;
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            User user = new User();

            try
            {
                user = userDao.GetUserById(id);
            }
            catch (DaoException)
            {
                StatusCode(500, "Dao Exception occurred");
            }
            return user;
        }
        [HttpGet("balance")]
        public ActionResult<Account> GetBalance()
        {
            string result = User.Identity.Name;

            Account account;
            try
            {
                account = userDao.GetBalanceByUsername(result);

            }
            catch (DaoException)
            {
                return BadRequest("An error occured and balance could not be displayed.");
            }
            return account;
        }

        //todo: changing values in the database despite throwing exceptions
        [HttpPost("send")]
        public ActionResult TransferFunds(Transfer transfer)
        {
            Transfer completeTransfer = null;
            try
            {
                User user = userDao.GetUserByUsername(User.Identity.Name);
                Account used;
                Account used2;
                if (transfer.AccountTo == 0 && transfer.TransferStatusId == 2)
                {
                    used = userDao.GetAccountByUserId(transfer.OtherUserId);
                    transfer.AccountTo = used.AccountId;
                }
                if (transfer.AccountFrom == 0 && transfer.TransferStatusId == 2)
                {
                    used2 = userDao.GetAccountByUserId(user.UserId);
                    transfer.AccountFrom = used2.AccountId;
                }
                if (transfer.AccountTo == 0)
                {
                    used = userDao.GetAccountByUserId(user.UserId);
                    transfer.AccountTo = used.AccountId;
                }
                if (transfer.AccountFrom == 0)
                {
                    used2 = userDao.GetAccountByUserId(transfer.OtherUserId);
                    transfer.AccountFrom = used2.AccountId;
                }
                
                User userFrom = userDao.GetUserByAccountId(transfer.AccountFrom);
                decimal userBalance = userDao.GetBalanceByUsername(userFrom.Username).Balance;
                
                if (userBalance - transfer.Amount >= 0 && transfer.Amount > 0 && transfer.AccountFrom != transfer.AccountTo)
                {

                    completeTransfer = userDao.CreateTransfer(transfer);
                    if (transfer.TransferStatusId == 2)
                    {
                        userDao.ChangeBalance(transfer.Amount, transfer.AccountTo);
                        userDao.ChangeBalance((transfer.Amount * -1), transfer.AccountFrom);
                    }
                }

            }
            catch (DaoException)
            {
                StatusCode(500, "Dao Exception Occurred");
            }
            return Ok(completeTransfer);
        }
        [HttpGet("/userlist")]
        public ActionResult<List<User>> UserList()
        {
            List<User> users = (List<User>)userDao.GetUsers();
            return users;
        }
        [HttpGet("transferlist")]
        public ActionResult<List<Transfer>> TransferList()
        {
            List<Transfer> transfers = new List<Transfer>();
            User user = userDao.GetUserByUsername(User.Identity.Name);
            Account account = userDao.GetAccountByUserId(user.UserId);
            transfers = userDao.TransferLog(account.AccountId);

            return Ok(transfers);
        }
        [HttpGet("transfer{id}")]
        public ActionResult<Transfer> GetTransferById(int id)
        {
            Transfer transfer = userDao.GetTransferByTransferId(id);
            User userTo = userDao.GetUserByAccountId(transfer.AccountTo);
            User userFrom = userDao.GetUserByAccountId(transfer.AccountFrom);
            transfer.AccountFromOwner = userFrom.Username;
            transfer.AccountToOwner = userTo.Username;

            return Ok(transfer);

        }
        [HttpGet("pending_requests")]
        public ActionResult<List<Transfer>> GetPendingRequests()
        {
            string username = User.Identity.Name;
            User user = userDao.GetUserByUsername(username);
            Account account = userDao.GetAccountByUserId(user.UserId);
            List<Transfer> transfers = userDao.GetPendingRequests(account.AccountId);
            return transfers;
        }
        [HttpPut("transfer_confirm")]
        public ActionResult<Transfer> ConfirmTransfer(Transfer transfer)
        {
            if(transfer.TransferStatusId == 2)
            {
                int toId = userDao.GetUserByAccountId(transfer.AccountTo).UserId;
                int fromId = userDao.GetUserByAccountId(transfer.AccountFrom).UserId;
                userDao.ChangeBalance(transfer.Amount * -1, fromId);
                userDao.ChangeBalance(transfer.Amount, toId);
            }
            Transfer updateTransfer = userDao.UpdateTransferStatus(transfer.TransferId, transfer.TransferStatusId);
            return updateTransfer;
        }
    }

}
