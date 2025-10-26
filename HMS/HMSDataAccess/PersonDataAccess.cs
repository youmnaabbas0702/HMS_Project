using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("Error in GetPersonInfoByID: " + ex.Message);
            }

            return isFound;
        }
    }
}
