using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Exceptions;
using Capstone.Models;

namespace Capstone.DAO
{
    
    public class CellSqlDao : ICellDao
    {
        private readonly string connectionString;
       
        public CellSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        
        public List<Cell> getCells()
        {
            List<Cell> cells = new List<Cell>();
            string sql = @"SELECT cell_id, x_cord, y_cord, letter_x, letter_y, color, acid
                                FROM cells
                            WHERE cells.color != '';
                          ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Cell cell = cell_mapper(reader);
                        cells.Add(cell);
                    }
                }
            } catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return cells;
        }

        public List<Cell> getPossibleCells(string letters)
        {
            List<Cell> cells = new List<Cell>();
            //Note(anita): Grab the Y letter and find all the possible X's
            string letterY = letters.Substring(0, 1);
            string sql = "SELECT cell_id, x_cord, y_cord, letter_x, letter_y, color, acid FROM cells WHERE letter_y = @letterY;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@letterY", letterY);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cell cell = cell_mapper(reader);
                        cells.Add(cell);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return cells;
        }
        

        public List<Cell> getCellByLetters(string letters)
        {
            List<Cell> cells = new List<Cell>();
            
            //Note(anita): This looks a bit weird but the look up is done by letter, not by cord
            //             and we look up Y first due to how the database is set up
            string letterY = letters.Substring(0, 1);
            string letterX = letters.Substring(1, 1);
            
            string sql = "SELECT cell_id, x_cord, y_cord, letter_x, letter_y, color, acid FROM cells WHERE letter_y = @letterY AND letter_x = @letterX;";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@letterY", letterY);
                    cmd.Parameters.AddWithValue("@letterX", letterX);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Cell cell = cell_mapper(reader);
                        cells.Add(cell);
                    }
                }
            } catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }


            return cells;
        }

        public List<Cell> getCellsByLetters(string letters)
        {
            throw new NotImplementedException();
        }

        public List<Cell> getFastestCells(string str)
        {
            string letter_y = str.Substring(0, 1);
            string letter_x = str.Substring(1, 1);
            
            List<Cell> cells = new List<Cell>();
            
            string sqlStatement = @"
                -- Calculate the distance and rank for each cell
                WITH DistanceCalculation AS (
                    SELECT
                        c.*,
                        ABS(c.x_cord - s.x_cord) + ABS(c.y_cord - s.y_cord) AS distance,
                        ROW_NUMBER() OVER (PARTITION BY c.color ORDER BY ABS(c.x_cord - s.x_cord) + ABS(c.y_cord - s.y_cord)) AS rnk
                    FROM
                        cells c
                    CROSS JOIN
                        (SELECT x_cord, y_cord FROM cells WHERE letter_x = @LetterX AND letter_y = @LetterY) s
                    WHERE
                        c.color IN ('blue', 'green', 'yellow')
                )
            
                -- Select the closest blue, green, and yellow cells
                SELECT cell_id, x_cord, y_cord, letter_x, letter_y, color, acid
                FROM DistanceCalculation
                WHERE rnk <= 3;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    SqlCommand cmd = new SqlCommand(sqlStatement, conn);
                    cmd.Parameters.AddWithValue("@LetterX", letter_x);
                    cmd.Parameters.AddWithValue("@LetterY", letter_y);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Cell cell = cell_mapper(reader);
                        cells.Add(cell);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }
            return cells;
        } 

        private Cell cell_mapper(SqlDataReader reader)
        {
            Cell cell = new Cell
            {
                CellId = Convert.ToInt32(reader["cell_id"]),
                CordX = Convert.ToInt32(reader["x_cord"]),
                CordY = Convert.ToInt32(reader["y_cord"]),
                LetterX = Convert.ToString(reader["letter_x"]),
                LetterY = Convert.ToString(reader["letter_y"]),
                Color = Convert.ToString(reader["color"]),
                Acid = Convert.ToString(reader["acid"])
            };
            return cell;
        }
    }
}