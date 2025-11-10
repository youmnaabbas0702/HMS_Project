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
    public class DoctorDataAccess
    {
        public static bool FindDoctorByID(
    int doctorID,
    ref int personID,
    ref int departmentID,
    ref string licenseNumber,
    ref byte experienceYears,
    ref DateTime dateJoined,
    ref bool isActive)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_FindDoctorByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            personID = Convert.ToInt32(reader["PersonID"]);
                            departmentID = Convert.ToInt32(reader["DepartmentID"]);
                            licenseNumber = reader["LicenseNumber"].ToString();
                            experienceYears = Convert.ToByte(reader["ExperienceYears"]);
                            dateJoined = Convert.ToDateTime(reader["DateJoined"]);
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

        public static bool AddNewDoctor(string NationalNo, string FullName, int NationalityID, DateTime DateOfBirth, bool Gender,
    string Address, string Phone, string Email, string PersonPicturePath, int DepartmentID,
    string LicenseNumber, byte ExperienceYears, DateTime DateJoined, bool IsActive,
    ref int NewPersonID, ref int NewDoctorID)
        {
            bool isAdded = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_AddNewDoctor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@NationalityID", NationalityID);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@PersonPicturePath", PersonPicturePath);
                    cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                    cmd.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                    cmd.Parameters.AddWithValue("@ExperienceYears", ExperienceYears);
                    cmd.Parameters.AddWithValue("@DateJoined", DateJoined);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);

                    // Output parameters
                    SqlParameter NewPersonIdParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter NewDoctorIdParam = new SqlParameter("@NewDoctorID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(NewPersonIdParam);
                    cmd.Parameters.Add(NewDoctorIdParam);

                    conn.Open();
                    int RowsAffected = cmd.ExecuteNonQuery();

                    if (RowsAffected > 0)
                    {
                        NewPersonID = Convert.ToInt32(NewPersonIdParam.Value);
                        NewDoctorID = Convert.ToInt32(NewDoctorIdParam.Value);
                        isAdded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);

                isAdded = false;

            }
            
            return isAdded;
        }

        public static bool UpdateDoctor(
    int DoctorID,
    string NationalNo,
    string FullName,
    int NationalityID,
    DateTime DateOfBirth,
    bool Gender,
    string Address,
    string Phone,
    string Email,
    string PersonPicturePath,
    int DepartmentID,
    string LicenseNumber,
    byte ExperienceYears,
    DateTime DateJoined,
    bool IsActive)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_UpdateDoctor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DoctorID", DoctorID);

                    // People table parameters
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@NationalityID", NationalityID);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@PersonPicturePath", PersonPicturePath);

                    // Doctor table parameters
                    cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
                    cmd.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                    cmd.Parameters.AddWithValue("@ExperienceYears", ExperienceYears);
                    cmd.Parameters.AddWithValue("@DateJoined", DateJoined);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);

                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();

                    isUpdated = affected > 0;
                }
            }

            catch (Exception ex)
            {
                LoggingManager.LogError(ex);

                return false;
            }

            return isUpdated;
        }

        public static DataTable GetAllDoctors()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_GetAllDoctors", conn))
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

        public static bool DeleteDoctorByPersonID(int personID)
        {
            bool isDeleted = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_DeleteDoctor_ByPersonID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isDeleted = rows > 0;
                }
            }

            catch (Exception ex)
            {
                LoggingManager.LogError(ex);
            }

            return isDeleted;
        }

        public static bool DeleteDoctorByDoctorID(int doctorID)
        {
            bool isDeleted = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_DeleteDoctor_ByDoctorID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isDeleted = rows > 0;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError(ex);
            }

            return isDeleted;
        }

        public static bool DeactivateDoctor(int doctorID)
        {
            bool isDeactivated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_DeactivateDoctor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    isDeactivated = rows > 0;
                }
            }

            catch (Exception ex)
            {
                LoggingManager.LogError(ex);
            }

            return isDeactivated;
        }

        public static bool IsDoctorExist(string LicenseNumber)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Doctors WHERE LicenseNumber = @LicenseNumber";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);

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


    }
}
