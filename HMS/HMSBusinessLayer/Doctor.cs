using HMSDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSBusinessLayer
{
    public class Doctor
    {
        public enum enMode { AddNew, Update }

        public int DoctorID { get; private set; }
        public int PersonID { get; private set; }
        public Person PersonInfo { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string LicenseNumber { get; set; }
        public byte ExperienceYears { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }

        public enMode Mode { get; private set; }

        public Doctor()
        {
            DoctorID = -1;
            PersonID = -1;
            PersonInfo = new Person();
            DepartmentID = -1;
            DepartmentName = "";
            LicenseNumber = "";
            ExperienceYears = 0;
            DateJoined = DateTime.Now;
            IsActive = true;

            Mode = enMode.AddNew;
        }

        private Doctor(int doctorID, int personID, int departmentID, string licenseNumber,
                          byte experienceYears, DateTime dateJoined, bool isActive)
        {
            DoctorID = doctorID;
            PersonID = personID;
            PersonInfo = Person.Find(personID);
            DepartmentID = departmentID;
            DepartmentName = "";
            LicenseNumber = licenseNumber;
            ExperienceYears = experienceYears;
            DateJoined = dateJoined;
            IsActive = isActive;

            Mode = enMode.Update;
        }

        public static Doctor Find(int doctorID)
        {
            int personID = -1;
            int departmentID = -1;
            string licenseNumber = "";
            byte experienceYears = 0;
            DateTime dateJoined = DateTime.MinValue;
            bool isActive = false;

            bool isFound = DoctorDataAccess.FindDoctorByID(
                doctorID,
                ref personID,
                ref departmentID,
                ref licenseNumber,
                ref experienceYears,
                ref dateJoined,
                ref isActive
            );

            if (isFound)
            {
                return new Doctor(
                    doctorID,
                    personID,
                    departmentID,
                    licenseNumber,
                    experienceYears,
                    dateJoined,
                    isActive
                );
            }

            return null;
        }

        private bool _AddNewDoctor()
        {
            int newPersonID = 0;
            int newDoctorID = 0;

            bool success = DoctorDataAccess.AddNewDoctor(
                PersonInfo.NationalNo,
                PersonInfo.FullName,
                PersonInfo.NationalityID,
                PersonInfo.DateOfBirth,
                PersonInfo.Gender,
                PersonInfo.Address,
                PersonInfo.Phone,
                PersonInfo.Email,
                PersonInfo.PersonPicturePath,
                DepartmentID,
                LicenseNumber,
                ExperienceYears,
                DateJoined,
                IsActive,
                ref newPersonID,
                ref newDoctorID
            );

            if (success)
            {
                PersonID = newPersonID;
                DoctorID = newDoctorID;
            }

            return success;
        }

        private bool _UpdateDoctor()
        {
            // Use the DAL function to update both People + Doctor info
            return DoctorDataAccess.UpdateDoctor(
                this.DoctorID,
                this.PersonInfo.NationalNo,
                this.PersonInfo.FullName,
                this.PersonInfo.NationalityID,
                this.PersonInfo.DateOfBirth,
                this.PersonInfo.Gender,
                this.PersonInfo.Address,
                this.PersonInfo.Phone,
                this.PersonInfo.Email,
                this.PersonInfo.PersonPicturePath,
                this.DepartmentID,
                this.LicenseNumber,
                this.ExperienceYears,
                this.DateJoined,
                this.IsActive
            );
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDoctor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateDoctor();
                default:
                    return false;
            }
        }

        public static List<Doctor> GetAllDoctorsList()
        {
            DataTable dt = DoctorDataAccess.GetAllDoctors();
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<Doctor> doctors = new List<Doctor>();

            foreach (DataRow row in dt.Rows)
            {
                Person person = new Person()
                {
                    PersonID = Convert.ToInt32(row["PersonID"]),
                    NationalNo = row["NationalNo"].ToString(),
                    FullName = row["FullName"].ToString(),
                    NationalityID = Convert.ToInt32(row["NationalityID"]),
                    CountryName = row["CountryName"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    Gender = Convert.ToBoolean(row["Gender"]),
                    Address = row["Address"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString()
                };

                Doctor doctor = new Doctor()
                {
                    DoctorID = Convert.ToInt32(row["DoctorID"]),
                    PersonID = person.PersonID,
                    PersonInfo = person,
                    DepartmentID = Convert.ToInt32(row["DepartmentID"]),
                    DepartmentName = row["DepartmentName"].ToString(),
                    LicenseNumber = row["LicenseNumber"].ToString(),
                    ExperienceYears = Convert.ToByte(row["ExperienceYears"]),
                    DateJoined = Convert.ToDateTime(row["DateJoined"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };

                doctors.Add(doctor);
            }

            return doctors;
        }



        public static bool DeleteByPersonID(int personID)
        {
            return DoctorDataAccess.DeleteDoctorByPersonID(personID);
        }

        public static bool DeleteByDoctorID(int doctorID)
        {
            return DoctorDataAccess.DeleteDoctorByDoctorID(doctorID);
        }

        public static bool DeactivateDoctor(int doctorID)
        {
            return DoctorDataAccess.DeactivateDoctor(doctorID);
        }

    }
}
