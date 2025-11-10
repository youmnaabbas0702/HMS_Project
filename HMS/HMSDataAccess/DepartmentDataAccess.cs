using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace HMSDataAccess
{
    public class DepartmentDataAccess
    {
        public static bool GetDepartmentInfoByID(int ID, ref string DepartmentName, ref string Description)
        {
            bool isFound = false;

            string query = "SELECT DepartmentName, Description FROM Departments WHERE DepartmentID = @DepartmentID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DepartmentID", ID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            DepartmentName = reader["DepartmentName"].ToString();
                            Description = reader["Description"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingManager.LogError(ex);

                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetDepartmentInfoByName(string DepartmentName, ref int DepartmentID, ref string Description)
        {
            bool isFound = false;

            string query = "SELECT DepartmentID, Description FROM Departments WHERE DepartmentName = @DepartmentName";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DepartmentName", DepartmentName);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                            Description = reader["Description"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingManager.LogError(ex);

                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAllDepartments()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Departments ORDER BY DepartmentName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingManager.LogError(ex);

                        return null;
                    }

                    return dt;
                }
            }
        }
    }
}
