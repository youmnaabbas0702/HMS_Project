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
    public class UserDataAccess
    {
        public static bool FindUserByID(int UserID,ref int personID,ref string UserName, ref int RoleID,ref bool isActive)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_FindUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            personID = Convert.ToInt32(reader["PersonID"]);
                            RoleID = Convert.ToInt32(reader["RoleID"]);
                            UserName = reader["UserName"].ToString();
                            isActive = Convert.ToBoolean(reader["IsActive"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggingManager.LogError(ex);
            }

            return isFound;
        }

        public static bool FindUserByUserName(
    string userName,
    ref int personID,
    ref int userID,
    ref int roleID,
    ref bool isActive)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_FindUserByUserName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            personID = Convert.ToInt32(reader["PersonID"]);
                            userID = Convert.ToInt32(reader["UserID"]);
                            roleID = Convert.ToInt32(reader["RoleID"]);
                            isActive = Convert.ToBoolean(reader["IsActive"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);
            }

            return isFound;
        }

        public static int AddNewUser(int personID, string userName, string passwordHash, int roleID, bool isActive)
        {
            int newUserID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddNewUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@PersonID", personID);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@RoleID", roleID);
                        cmd.Parameters.AddWithValue("@IsActive", isActive);

                        // Output parameter
                        SqlParameter outputParam = new SqlParameter("@NewUserID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Get the output value
                        newUserID = Convert.ToInt32(outputParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);
            }

            return newUserID;
        }

        public static bool UpdateUser(int userID, int personID, string userName, string passwordHash, int roleID, bool isActive)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@PersonID", personID);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@RoleID", roleID);
                        cmd.Parameters.AddWithValue("@IsActive", isActive);

                        // Output parameter
                        SqlParameter outputParam = new SqlParameter("@IsUpdated", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        isUpdated = Convert.ToBoolean(outputParam.Value);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error in UpdateUser: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateUser: " + ex.Message);
            }

            return isUpdated;
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        
        public static bool IsUserNameUsed(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool UserExists(string UserName, string Password)
        {
            bool isFound = false;
            string query = "SELECT Found = 1 FROM Users WHERE UserName = @UserName AND PasswordHash = @Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);

                return false;
            }
            return isFound;
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetAllUsers", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);

                return null;
            }

            return dt;
        }

        public static bool DeleteUser(int userID)
        {
            bool isDeleted = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    string query = "DELETE FROM Users WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        isDeleted = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError (ex);
            }

            return isDeleted;
        }

    }
}
