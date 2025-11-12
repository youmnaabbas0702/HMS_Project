using HMS_DesktopApp.Properties;
using HMSBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMS_DesktopApp.Doctors
{
    public partial class ctrlDoctorCard : UserControl
    {
        private Doctor _Doctor;

        private int _DoctorID = -1;

        public int DoctorID
        {
            get { return _DoctorID; }
        }

        public Doctor SelectedDoctorInfo
        {
            get { return _Doctor; }
        }


        public ctrlDoctorCard()
        {
            InitializeComponent();
        }

        public void LoadDoctorInfo(int DoctorID)
        {
            _Doctor = Doctor.Find(DoctorID);
            if (_Doctor == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Doctor with ID = " + DoctorID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillDoctorInfo();
        }

        
        private void _LoadPersonImage()
        {
            if (_Doctor.PersonInfo.Gender)
                pbImage.Image = Resources.doctor_male_pp;
            else
                pbImage.Image = Resources.doctor_female_pp;

            string ImagePath = _Doctor.PersonInfo.PersonPicturePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _FillDoctorInfo()
        {
            llEditDoctorInfo.Enabled = true;
            _DoctorID = _Doctor.DoctorID;
            lblPersonID.Text = _Doctor.PersonID.ToString();
            lblDoctorID.Text = _Doctor.DoctorID.ToString();
            lblNationalNo.Text = _Doctor.PersonInfo.NationalNo;
            lblName.Text = _Doctor.PersonInfo.FullName;
            lblGender.Text = _Doctor.PersonInfo.Gender ? "Male" : "Female";
            pbGender.Image = _Doctor.PersonInfo.Gender? Resources.doctor_male : Resources.doctor_female;
            lblEmail.Text = _Doctor.PersonInfo.Email;
            lblPhone.Text = _Doctor.PersonInfo.Phone;
            lblBirthDate.Text = _Doctor.PersonInfo.DateOfBirth.ToShortDateString();
            lblCountry.Text = Country.Find(_Doctor.PersonInfo.NationalityID).CountryName;
            lblAddress.Text = _Doctor.PersonInfo.Address;

            lblLicenseNumber.Text = _Doctor.LicenseNumber;
            lblExperience.Text = _Doctor.ExperienceYears.ToString();
            lblDepartment.Text = Department.Find(_Doctor.DepartmentID).DepartmentName;
            lblDateJoined.Text = _Doctor.DateJoined.ToShortDateString();
            _LoadPersonImage();

        }

        public void ResetPersonInfo()
        {
            _DoctorID = -1;
            lblPersonID.Text = "[????]";
            lblDoctorID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            pbGender.Image = Resources.doctor_male;
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblBirthDate.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbImage.Image = Resources.doctor_male_pp;

            lblLicenseNumber.Text = "[????]";
            lblExperience.Text = "[????]";
            lblDepartment.Text = "[????]";
            lblDateJoined.Text = "[????]";
            
        }

        private void llEditDoctorInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditDoctor frm = new frmAddEditDoctor(_DoctorID);
            frm.ShowDialog();

            //refresh
            LoadDoctorInfo(_DoctorID);
        }
    }
}
