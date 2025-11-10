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
    public class PersonDataAccess
    {
        public static bool GetPersonInfoByID(
        int personID,
        ref string nationalNo,
        ref string fullName,
        ref int nationalityID,
        ref DateTime dateOfBirth,
        ref bool gender,
        ref string address,
        ref string phone,
        ref string email,
        ref string picturePath)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SP_FindPersonByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonID", personID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            nationalNo = reader["NationalNo"].ToString();
                            fullName = reader["FullName"].ToString();
                            nationalityID = Convert.ToInt32(reader["NationalityID"]);
                            dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            gender = Convert.ToBoolean(reader["Gender"]);
                            address = reader["Address"].ToString();
                            phone = reader["Phone"].ToString();
                            email = reader["Email"].ToString();
                            picturePath = reader["PersonPicturePath"].ToString();
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

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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
    }
}
