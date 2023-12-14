using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Exceptions;
using Capstone.Models;

namespace Capstone.DAO
{
    public class HomeSqlDao : IHomeDao
    {
        private readonly string connectionString;

        public HomeSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Home GetView()
        {
            
            string sql = "SELECT * FROM homeview WHERE active = 1";
            Home home = new Home();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        home = MapRowToHome(reader);
                        
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return home;
        }
        public Home GetViewById(int id)
        {

            string sql = "SELECT * FROM homeview WHERE view_id = @id";
            Home home = new Home();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id",id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        home = MapRowToHome(reader);

                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return home;
        }
        public List<Home> GetAllViews()
        {
            string sql = "SELECT * FROM homeview";
            List<Home> homes = new List<Home>();
            try {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Home home = MapRowToHome(reader);
                        homes.Add(home);
                    }
                }
                    }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return homes;

        }
        public Home PostNewHomeView(Home data)
        {
            string sql = "INSERT INTO homeview (header, body, image_source, active, name) " +
            "OUTPUT INSERTED.view_id " +
            "VALUES (@header, @body, @image_source, @active, @name) ";
            Home newHome = new Home();
            int newId = 0;
            data = NullPropertyToEmpty(data);
            try {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@header", data.Header);
                    cmd.Parameters.AddWithValue("@body", data.Body);
                    cmd.Parameters.AddWithValue("@image_source", data.Image);
                    cmd.Parameters.AddWithValue("@active", false);
                    cmd.Parameters.AddWithValue("@name", data.Name);
                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                    newHome = GetViewById(newId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return newHome;

        } 
        public void UpdateHomeView(int id)
        {
            string sql = "UPDATE homeview " +
             "SET active = 0 " +
             "WHERE active = 1 " + 
             "UPDATE homeview " +
             "SET active = 1 " +
             "WHERE view_id = @id ";
            if (id == -1)
            {
                id = GetNextId() -1;
            }
            try { 
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
        }
        public int GetNextId()
        {
            int nextId = 0;
            string sql = "SELECT IDENT_CURRENT ('homeview') AS last_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        nextId = (1 + Convert.ToInt32(reader["last_id"]));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return nextId;
        }
        public bool DeleteViewById(int id)
        {
            Home home = GetViewById(id);
            if (home == null)
            {
                return false;
            }
            bool result = false;
            string sql = "DELETE FROM homeview " +
            "WHERE view_id = @view_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@view_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            result = true;
            return result;
        }
        private Home NullPropertyToEmpty(Home home)
        {
            if(home.Active == null)
            {
                home.Active = false;
            }
            if(home.Body == null || home.Body == "")
            {
                home.Body = "empty";
            }
            if(home.Header == null || home.Header == "")
            {
                home.Header = "empty";
            }
            if(home.Image == null || home.Image == "")
            {
                home.Image = "src/img/Empty.jpg";
            }
            if(home.Name == null || home.Name == "")
            {
                home.Name = GetNextId().ToString();
            }
            return home;
        }
        private Home MapRowToHome(SqlDataReader reader)
        {
            Home home = new Home();
            home.ViewId = Convert.ToInt32(reader["view_id"]);
            home.Body = Convert.ToString(reader["body"]);
            home.Header = Convert.ToString(reader["header"]);
            home.Image = Convert.ToString(reader["image_source"]);
            home.Active = Convert.ToBoolean(reader["active"]);
            home.Name = Convert.ToString(reader["name"]);
            return home;
        }
    }
}
