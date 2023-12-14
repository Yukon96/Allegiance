using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Capstone.Exceptions;
using Capstone.Models;

namespace Capstone.DAO
{
    public class ProteinSqlDao : IProteinDao
    {
        private readonly string connectionString;

        public ProteinSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        
        public IList<Protein> GetProteins()
        {
            IList<Protein> proteins = new List<Protein>();

            string sql = "SELECT protein_id, sequence_name, protein_sequence, description, format_type, username, user_id, sequence_1, sequence_2, sequence_3  FROM proteins";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Protein protein = MapRowToProtein(reader);
                        proteins.Add(protein);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return proteins;
        }

        public Protein GetProteinById(int id)
        {
            Protein protein = null;

            string sql = "SELECT protein_id, sequence_name, protein_sequence, description, format_type, username, user_id, sequence_1, sequence_2, sequence_3 " +
                "FROM proteins " +
                "WHERE protein_id = @protein_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@protein_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        protein = MapRowToProtein(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return protein;
        }

        public IList<Protein> GetProteinsBySequenceName(string sequenceName)
        {
            {
                IList<Protein> proteins = new List<Protein>();
                string sql = "SELECT protein_id, sequence_name, protein_sequence, description, format_type, username, user_id, sequence_1, sequence_2, sequence_3 " +
                    "FROM proteins " +
                    "WHERE sequence_name = @sequence_name";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@sequence_name", sequenceName);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Protein protein = MapRowToProtein(reader);
                            proteins.Add(protein);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new DaoException("SQL exception occurred", ex);
                }

                return proteins;
            }
        }

        public IList<Protein> GetProteinsByUsername(string name)
        {
            IList<Protein> proteins = new List<Protein>();
            string sql = "SELECT protein_id, sequence_name, protein_sequence, description, format_type, username, user_id, sequence_1, sequence_2, sequence_3 " +
                "FROM proteins " +
                "WHERE username = @username";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Protein protein = MapRowToProtein(reader);
                        proteins.Add(protein);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return proteins;
        }


        public Protein CreateProtein(string sequenceName, string proteinSequence, string description, string username, int userId)
        {
            Protein newProtein = NullPropertyToEmpty(sequenceName, proteinSequence, description, userId);
            int formatType = DetectFormat(proteinSequence);
            string sql = "INSERT INTO proteins (sequence_name, protein_sequence, description, format_type, username, user_id) " +
                "OUTPUT INSERTED.protein_id " +
                "VALUES (@sequence_name, @protein_sequence, @description, @format_type, @username, @user_id)";
            int newProteinId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sequence_name", newProtein.SequenceName);
                    cmd.Parameters.AddWithValue("@protein_sequence", newProtein.ProteinSequence);
                    cmd.Parameters.AddWithValue("@description", newProtein.Description);
                    cmd.Parameters.AddWithValue("@format_type", formatType);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@user_id", newProtein.UserId);

                    newProteinId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                newProtein = GetProteinById(newProteinId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return newProtein;
        }

        public Protein UpdateProtein(int proteinId, string sequenceName, string proteinSequence, string description, int userId)
        {
            Protein updatedProtein = null;
            int formatType = DetectFormat(proteinSequence);
            string sql = "UPDATE proteins " +
            "SET sequence_name = @sequence_name, protein_sequence = @protein_sequence, description = @description, format_type = @format_type, username = (SELECT username FROM users WHERE user_id = @user_id), user_id = @user_id " +
            "WHERE protein_id = @protein_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sequence_name", sequenceName);
                    cmd.Parameters.AddWithValue("@protein_sequence", proteinSequence);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@format_type", formatType);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@protein_id", proteinId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            updatedProtein = GetProteinById(proteinId);
            return updatedProtein;
        }

        public ProteinResponse OptimizeProtein(Protein protein)
        {
            string sql = "UPDATE proteins " + 
                         "SET sequence_1 = @sequence_1, sequence_2 = @sequence_2, sequence_3 = @sequence_3 " + 
                         "WHERE protein_id = @protein_id";

            ProteinResponse protein_response;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sequence_1", protein.Sequence1);
                    cmd.Parameters.AddWithValue("@sequence_2", protein.Sequence2);
                    cmd.Parameters.AddWithValue("@sequence_3", protein.Sequence3);
                    cmd.Parameters.AddWithValue("@protein_id", protein.ProteinId);
                    cmd.ExecuteNonQuery();


                    protein_response = new ProteinResponse();
                    protein_response.ProteinId = protein.ProteinId;
                    protein_response.SequenceName = protein.SequenceName;
                    protein_response.ProteinSequence = protein.ProteinSequence;
                    protein_response.Description = protein.Description;
                    protein_response.FormatType = protein.FormatType;
                    protein_response.UserId = protein.UserId;
                    string[] blues = protein.Sequence1.Split(",");
                    string[] greens = protein.Sequence2.Split(",");
                    string[] yellow = protein.Sequence3.Split(",");
                    protein_response.BlueSequence = new List<string>(blues);
                    protein_response.GreenSequence = new List<string>(greens);
                    protein_response.YellowSequence = new List<string>(yellow);
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            

            return protein_response;
        }


        public bool DeleteProteinById(int proteinId)
        {
            Protein protein = GetProteinById(proteinId);
            if (protein == null)
            {
                return false;
            }
            bool result = false;
            string sql = "DELETE FROM proteins " +
            "WHERE protein_id = @protein_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@protein_id", proteinId);
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

        public int DetectFormat(string sequence)
        {
            int result = 0;
            if (sequence != "")
            {
                if (Char.IsDigit(sequence, 0))
                {
                    result = 3;
                }
                else if (sequence.IndexOf(" ") == 10)
                {
                    result = 2;
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }
        
        public Protein NullPropertyToEmpty(string sequenceName, string proteinSequence, string description, int userId)
        {
            Protein protein = new Protein();
            if (description == "" || description == null)
            {
                protein.Description = "No Note";
            }
            else protein.Description = description;
            if (proteinSequence == "" || proteinSequence == null)
            {
                protein.ProteinSequence = "No Sequence";
            }
            else protein.ProteinSequence = proteinSequence;
            if (sequenceName == "" || sequenceName == null)
            {
                protein.SequenceName = "No Name";
            }
            else protein.SequenceName = sequenceName;
            if (userId == 0)
            {
                protein.UserId = 2;
            }
            else protein.UserId = userId;
            return protein;
        }

        private Protein MapRowToProtein(SqlDataReader reader)
        {
            Protein protein = new Protein();
            protein.ProteinId = Convert.ToInt32(reader["protein_id"]);
            protein.SequenceName = Convert.ToString(reader["sequence_name"]);
            protein.ProteinSequence = Convert.ToString(reader["protein_sequence"]);
            protein.Description = Convert.ToString(reader["description"]);
            protein.FormatType = Convert.ToInt32(reader["format_type"]);
            protein.UserId = Convert.ToInt32(reader["user_id"]);
            //set possible null values to empty string
            protein.Sequence1 = reader["sequence_1"] is DBNull ? "" : Convert.ToString(reader["sequence_1"]);
            protein.Sequence2 = reader["sequence_2"] is DBNull ? "" : Convert.ToString(reader["sequence_2"]);
            protein.Sequence3 = reader["sequence_3"] is DBNull ? "" : Convert.ToString(reader["sequence_3"]);

            return protein;
        }
    }
}
