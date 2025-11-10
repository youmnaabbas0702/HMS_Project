using HMSDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSBusinessLayer
{
    public class Person
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public int NationalityID { get; set; }
        public string CountryName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PersonPicturePath { get; set; }

        public Person()
        {
            PersonID = -1;
            NationalNo = "";
            FullName = "";
            NationalityID = -1;
            CountryName = "";
            DateOfBirth = DateTime.Now;
            Gender = true;
            Address = "";
            Phone = "";
            Email = "";
            PersonPicturePath = "";
        }

        private Person(int personID, string nationalNo, string fullName, int nationalityID,
                          DateTime dateOfBirth, bool gender, string address, string phone,
                          string email, string personPicturePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            FullName = fullName;
            NationalityID = nationalityID;
            CountryName = "";
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            PersonPicturePath = personPicturePath;
        }

        public static Person Find(int personID)
        {
            string nationalNo = "", fullName = "", address = "", phone = "", email = "", picturePath = "";
            int nationalityID = -1;
            DateTime dob = DateTime.Now;
            bool gender = true;

            bool isFound = PersonDataAccess.GetPersonInfoByID(personID, ref nationalNo, ref fullName,
                ref nationalityID, ref dob, ref gender, ref address, ref phone, ref email, ref picturePath);

            if (isFound)
            {
                return new Person(personID, nationalNo, fullName, nationalityID, dob, gender, address, phone, email, picturePath);
            }

            return null;
        }

        public static bool isPersonExist(string NationlNo)
        {
            return PersonDataAccess.IsPersonExist(NationlNo);
        }
    }
}
