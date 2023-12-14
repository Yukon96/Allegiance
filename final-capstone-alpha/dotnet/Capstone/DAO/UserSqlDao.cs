using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;
using Microsoft.AspNetCore.Identity;

namespace Capstone.DAO
{
    public class UserSqlDao : IUserDao
    {
        private readonly string connectionString;

        public UserSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<ReturnUser> GetUsers()
        {
            IList<ReturnUser> users = new List<ReturnUser>();

            string sql = "SELECT user_id, username, email, user_role FROM users";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ReturnUser user = MapRowToReturnUser(reader);
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

        public User GetUserById(int userId)
        {
            User user = null;

            string sql = "SELECT user_id, username, email, organization_name, password_hash, salt, user_role, has_one_time_password FROM users WHERE user_id = @user_id";

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


        public User GetFullUserByUsername(string username)
        {
            User user = null;

            string sql = "SELECT user_id, username, email, organization_name, user_role, password_hash, salt, has_one_time_password FROM users WHERE username = @username";

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

        public User GetFullUserByEmail(string email)
        {
            User user = null;

            string sql = "SELECT user_id, username, email, organization_name, user_role, password_hash, salt, has_one_time_password FROM users WHERE email = @email";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", email);
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

        public ReturnUser GetUserByUsername(string username)
        {
            ReturnUser user = null;

            string sql = "SELECT user_id, username, user_role, email FROM users WHERE username = @username";

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
                        user = MapRowToReturnUser(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return user;
        }

        public User CreateUser(string username, string email, string organizationName, string password, string role)
        {
            User newUser = new User();
            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(password);
            int hasOneTimePassword = 0;

            string sql = "INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) " +
                         "OUTPUT INSERTED.user_id " +
                         "VALUES (@username, @email, @organization_name, @password_hash, @salt, @user_role, @has_one_time_password)";
            int newUserId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@organization_name", organizationName);
                    cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                    cmd.Parameters.AddWithValue("@salt", hash.Salt);
                    cmd.Parameters.AddWithValue("@user_role", role);
                    cmd.Parameters.AddWithValue("@has_one_time_password", hasOneTimePassword);
                    newUserId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                newUser = GetUserById(newUserId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return newUser;
        }

        public void ChangePassword(string username, string password)
        {
            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(password);
            bool hasOneTimePassword = false;
            string sql = "UPDATE users " +
             "SET password_hash = @password_hash, salt = @salt, has_one_time_password = @has_one_time_password " +
             "WHERE username = @username";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                    cmd.Parameters.AddWithValue("@salt", hash.Salt);
                    cmd.Parameters.AddWithValue("@has_one_time_password", hasOneTimePassword);

                    cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }

        public string GenerateOneTimePassword(string username)
        {   
            Console.WriteLine(username);
            PasswordGenerator generator = new PasswordGenerator();
            IPasswordHasher passwordHasher = new PasswordHasher();

            string oneTimePassword = generator.oneTimeGenerator();
            PasswordHash hash = passwordHasher.ComputeHash(oneTimePassword);
            bool hasOneTimePassword = true;
            string sql = "UPDATE users " +
             "SET password_hash = @password_hash, salt = @salt, has_one_time_password = @has_one_time_password " +
             "WHERE username = @username";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                    cmd.Parameters.AddWithValue("@salt", hash.Salt);
                    cmd.Parameters.AddWithValue("@has_one_time_password", hasOneTimePassword);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return oneTimePassword;
        }

        private User MapRowToUser(SqlDataReader reader)
        {
            User user = new User();
            user.UserId = Convert.ToInt32(reader["user_id"]);
            user.Username = Convert.ToString(reader["username"]);
            user.Email = Convert.ToString(reader["email"]);
            user.OrganizationName = Convert.ToString(reader["organization_name"]);
            user.Role = Convert.ToString(reader["user_role"]);
            user.PasswordHash = Convert.ToString(reader["password_hash"]);
            user.Salt = Convert.ToString(reader["salt"]);
            user.HasOneTimePassword = Convert.ToBoolean(reader["has_one_time_password"]);
            return user;
        }

        private ReturnUser MapRowToReturnUser(SqlDataReader reader)
        {
            ReturnUser user = new ReturnUser();
            user.UserId = Convert.ToInt32(reader["user_id"]);
            user.Username = Convert.ToString(reader["username"]);
            user.Email = Convert.ToString(reader["email"]);
            user.Role = Convert.ToString(reader["user_role"]);
            return user;
        }
    }
}
