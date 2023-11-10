using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class UserSqlDao : IUserDao
    {
        private readonly string connectionString;
        const decimal StartingBalance = 1000M;

        public UserSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public User GetUserById(int userId)
        {
            User user = null;

            string sql = "SELECT user_id, username, password_hash, salt FROM tenmo_user WHERE user_id = @user_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return user;
        }

        public User GetUserByUsername(string username)
        {
            User user = null;

            string sql = "SELECT user_id, username, password_hash, salt FROM tenmo_user WHERE username = @username";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return user;
        }

        public IList<User> GetUsers()
        {
            IList<User> users = new List<User>();

            string sql = "SELECT user_id, username, password_hash, salt FROM tenmo_user";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = MapRowToUser(reader);
                        users.Add(user);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return users;
        }

        public User CreateUser(string username, string password)
        {
            User newUser = null;

            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(password);

            string sql = "INSERT INTO tenmo_user (username, password_hash, salt) " +
                         "OUTPUT INSERTED.user_id " +
                         "VALUES (@username, @password_hash, @salt)";

            int newUserId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // create user
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                    cmd.Parameters.AddWithValue("@salt", hash.Salt);

                    newUserId = Convert.ToInt32(cmd.ExecuteScalar());

                    // create account
                    cmd = new SqlCommand("INSERT INTO account (user_id, balance) VALUES (@userid, @startBalance)", conn);
                    cmd.Parameters.AddWithValue("@userid", newUserId);
                    cmd.Parameters.AddWithValue("@startBalance", StartingBalance);
                    cmd.ExecuteNonQuery();
                }
                newUser = GetUserById(newUserId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return newUser;
        }

        public Account GetBalanceByUsername(string username)
        {
            {


                Account account = null;

                string sql = "SELECT * FROM account " +
                    "JOIN tenmo_user ON tenmo_user.user_id = account.user_id " +
                    "WHERE username = @username";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@username", username);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            account = MapRowToAccount(reader);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new DaoException("SQL exception occurred", ex);
                }

                return account;
            }
        }

        public List<User> ListUsers()
        {
            List<User> users = new List<User>();

            string sqlListUsers = "SELECT * FROM tenmo_user";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlListUsers, conn))
                    {

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                User user = new User();
                                user = MapRowToUser(reader);
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }


            return users;
        }
        //todo: transfer type and transfer status hardcoded in the user interface
        public Transfer CreateTransfer(Transfer newtransfer)
        {
            Transfer transfer = new Transfer();
            string sql = "INSERT INTO transfer (transfer_type_id, transfer_status_id, " +
                " account_from, account_to, amount) " +
                "OUTPUT INSERTED.transfer_id " +
                "VALUES (@transfer_type_id, @transfer_status_id, " +
                "@account_from, @account_to, @amount)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@transfer_type_id", newtransfer.TransferTypeId);
                        cmd.Parameters.AddWithValue("@account_from", newtransfer.AccountFrom);
                        cmd.Parameters.AddWithValue("@account_to", newtransfer.AccountTo);
                        cmd.Parameters.AddWithValue("@transfer_status_id", newtransfer.TransferStatusId);
                        cmd.Parameters.AddWithValue("@amount", newtransfer.Amount);
                        int newId = (int)cmd.ExecuteScalar();
                        transfer = GetTransferByTransferId(newId);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            
            return transfer;
        }
        public Account GetAccountByUserId(int id)
        {
            Account account = new Account();
            string sql = "SELECT * FROM account WHERE user_id = @user_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("user_id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            account = MapRowToAccount(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return account;
        }
        public void ChangeBalance(decimal amount, int accountId)
        {
            string sql = "UPDATE account " +
                "SET balance += @amount " +
                "WHERE account_id = @account_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@account_id", accountId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

        }
        private User MapRowToUser(SqlDataReader reader)
        {
            User user = new User();
            user.UserId = Convert.ToInt32(reader["user_id"]);
            user.Username = Convert.ToString(reader["username"]);
            user.PasswordHash = Convert.ToString(reader["password_hash"]);
            user.Salt = Convert.ToString(reader["salt"]);
            return user;
        }
        private Account MapRowToAccount(SqlDataReader reader)
        {
            Account account = new Account();
            account.Balance = Convert.ToDecimal(reader["balance"]);
            account.AccountId = Convert.ToInt32(reader["account_id"]);
            account.UserId = Convert.ToInt32(reader["user_id"]);
            return account;
        }
        public List<Transfer> TransferLog(int accountid)
        {
            List<Transfer> transfers = new List<Transfer>();
            string sql = "SELECT * FROM transfer WHERE account_from = @id OR account_to = @id ;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", accountid);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Transfer transfer = new Transfer();
                            transfer = MapRowToTransfer(reader);
                            transfer.AccountFromOwner = GetUserByAccountId(transfer.AccountFrom).Username;
                            transfer.AccountToOwner = GetUserByAccountId(transfer.AccountTo).Username;
                            transfers.Add(transfer);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL Exepection occurred", ex);
            }
            return transfers;
        }
        public Transfer GetTransferByTransferId(int id)
        {
            Transfer transfer = new Transfer();
            string sql = "SELECT * FROM transfer WHERE transfer_id = @id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            transfer = MapRowToTransfer(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return transfer;
        }
        public List<Transfer> GetPendingRequests(int account_id)
        {
            List<Transfer> transfers = new List<Transfer>();
            string sql = "SELECT * FROM transfer WHERE transfer_status_id = @id AND account_from = @account_id";
            try
            {
                int id = 1;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@account_id", account_id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Transfer transfer;
                            transfer = MapRowToTransfer(reader);
                            transfers.Add(transfer);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return transfers;
        }
        public User GetUserByAccountId(int id)
        {
            User user = new User();
            string sql = "SELECT * FROM tenmo_user " +
                "JOIN account ON account.user_id = tenmo_user.user_id "
                + "WHERE account_id = @id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {

                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            user = MapRowToUser(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return user;
        }
        public Transfer UpdateTransferStatus(int transferId, int transferStatus)
        {
            string sql = "UPDATE Transfer " +
                "SET transfer_status_id = @transferStatus " +
                "WHERE transfer_id = @id";
            Transfer transfer;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", transferId);
                    cmd.Parameters.AddWithValue("transferStatus", transferStatus);
                    SqlDataReader reader = cmd.ExecuteReader();
                    transfer = MapRowToTransfer(reader);
                }
            }
            return transfer;
        }

        private Transfer MapRowToTransfer(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.Amount = Convert.ToInt32(reader["amount"]);
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.AccountFromOwner = GetUserByAccountId(transfer.AccountFrom).Username;
            transfer.AccountToOwner = GetUserByAccountId(transfer.AccountTo).Username;
            return transfer;
        }
    }
}
