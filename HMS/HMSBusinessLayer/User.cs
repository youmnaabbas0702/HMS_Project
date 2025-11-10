using HMSDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSBusinessLayer
{
    public class User
    {
        public enum enMode { AddNew, Update }
        public enMode Mode { get; private set; }
        public int UserID { get; private set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        //Add Role object
        public bool IsActive { get; set; }
        //We should have permissions

        public User()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = true;
            RoleID = -1;
            Mode = enMode.AddNew;
        }

        private User(int userID, int personID, string userName, string password, int roleID, bool isActive)
        {
            Mode = enMode.Update;
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Password = password;
            RoleID = roleID;
            IsActive = isActive;
        }

        public static User Find(int UserID)
        {
            int personID = -1;
            int RoleID = -1;
            string UserName = "";
            bool isActive = false;

            bool isFound = UserDataAccess.FindUserByID(
                UserID,
                ref personID,
                ref UserName,
                ref RoleID,
                ref isActive
            );

            if (isFound)
            {
                return new User(
                    UserID,
                    personID,
                    UserName,
                    "",
                    RoleID,
                    isActive
                );
            }

            return null;
        }

        private bool _AddNewUser()
        {
            // Hash before saving
            string hashedPassword = SecurityHelper.HashPassword(Password);

            UserID = UserDataAccess.AddNewUser(PersonID, UserName, hashedPassword, RoleID, IsActive);
            return UserID != -1;
        }

        private bool _UpdateUser()
        {
            // Hash before saving
            string hashedPassword = SecurityHelper.HashPassword(Password);

            return UserDataAccess.UpdateUser(UserID, PersonID, UserName, hashedPassword, RoleID, IsActive);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateUser();
                default:
                    return false;
            }
        }

        public static List<User> GetAllUsersList()
        {
            DataTable dt = UserDataAccess.GetAllUsers();
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<User> Users = new List<User>();

            foreach (DataRow row in dt.Rows)
            {
                User user = new User()
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    PersonID = Convert.ToInt32(row["PersonID"]),
                    RoleID = Convert.ToInt32(row["RoleID"]),
                    UserName = Convert.ToString(row["UserName"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };

                Users.Add(user);
            }

            return Users;
        }

        public static bool IsPersonAUser(int personID)
        {
            return UserDataAccess.IsUserExistByPersonID(personID);
        }

        public static bool IsUserExist(string UserName)
        {
            return UserDataAccess.IsUserNameUsed(UserName);
        }

        public static bool DeleteUser(int UserID)
        {
            return UserDataAccess.DeleteUser(UserID);
        }

        public static bool CheckUserCredentials(string userName, string password)
        {
            // Hash entered password before checking
            string hashedPassword = SecurityHelper.HashPassword(password);
            return UserDataAccess.UserExists(userName, hashedPassword);
        }
    }
}
