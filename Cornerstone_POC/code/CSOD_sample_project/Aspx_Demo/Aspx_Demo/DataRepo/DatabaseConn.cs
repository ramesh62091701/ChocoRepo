using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Aspx_Demo.DataRepo
{
    public class DatabaseConn
    {
        private readonly string connectionString;

        public DatabaseConn(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string GetAssignUserData()
        {
            StringBuilder result = new StringBuilder();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM AssignUserData";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string userName = reader.GetString(1);

                                result.AppendLine($"UserId: {userId}, UserName: {userName}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"An error occurred: {ex.Message}");
            }

            return result.ToString();
        }

        public int GetAssignUserCount()
        {
            int count = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM AssignUserData";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (omitted for brevity)
            }

            return count;
        }

        public bool InsertAssignUser(string userName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO AssignUserData (UserName) VALUES (@UserName)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteAssignUser(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM AssignUserData WHERE UserId = @UserId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}