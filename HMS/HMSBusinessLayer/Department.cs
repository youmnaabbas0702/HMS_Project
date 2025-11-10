using HMSDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSBusinessLayer
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }

        // Default constructor (for AddNew mode)
        public Department()
        {
            this.DepartmentID = -1;
            this.DepartmentName = string.Empty;
            this.Description = string.Empty;
        }

        // Private constructor (used by Find methods)
        private Department(int departmentID, string departmentName, string description)
        {
            this.DepartmentID = departmentID;
            this.DepartmentName = departmentName;
            this.Description = description;
        }

        // Find by ID
        public static Department Find(int departmentID)
        {
            string departmentName = string.Empty;
            string description = string.Empty;

            if (DepartmentDataAccess.GetDepartmentInfoByID(departmentID, ref departmentName, ref description))
                return new Department(departmentID, departmentName, description);
            else
                return null;
        }

        // Find by Name
        public static Department Find(string departmentName)
        {
            int departmentID = -1;
            string description = string.Empty;

            if (DepartmentDataAccess.GetDepartmentInfoByName(departmentName, ref departmentID, ref description))
                return new Department(departmentID, departmentName, description);
            else
                return null;
        }

        // Get all departments
        public static DataTable GetAllDepartments()
        {
            return DepartmentDataAccess.GetAllDepartments();
        }
    }
}
