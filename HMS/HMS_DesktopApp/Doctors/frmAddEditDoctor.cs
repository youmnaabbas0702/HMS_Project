using Common;
using HMS_DesktopApp.Global_Classes;
using HMS_DesktopApp.Properties;
using HMS_DesktopApp.Users;
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
    public partial class frmAddEditDoctor : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int DoctorID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _DoctorID = -1;
        Doctor _Doctor;

        public frmAddEditDoctor()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _Mode = enMode.AddNew;

        }

        public frmAddEditDoctor(int DoctorID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _Mode = enMode.Update;
            _DoctorID = DoctorID;
        }
        
        //initializing form data
        private void _ResetDefualtValues()
        {
            _FillCountriesInComoboBox();

            _FillDepartmentsInComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Doctor";
                _Doctor = new Doctor();
            }
            else
            {
                lblTitle.Text = "Update Doctor";
            }

            //set default image for the person.
            rbMale.Checked = true;
            pbImage.Image = Resources.doctor_male_pp;


            btnRemoveImage.Visible = (pbImage.ImageLocation != null);

            //we set the max date to 23 years from today, and set the default value the same.
            dtpBirthDate.MaxDate = DateTime.Now.AddYears(-23);
            dtpBirthDate.Value = dtpBirthDate.MaxDate;

            //should not allow adding age more than 100 years
            dtpBirthDate.MinDate = DateTime.Now.AddYears(-80);

            //we set the max date of hiring to today, and set the default value the same.
            dtpDateJoined.MaxDate = DateTime.Now;
            dtpDateJoined.Value = dtpDateJoined.MaxDate;

            //should not allow adding hire date more than 30 years ago (Optional)
            dtpDateJoined.MinDate = DateTime.Now.AddYears(-30);

            nupdYearsOfExperience.Maximum = 50;
            
            //this will set default country to egypt.
            cmbCountry.SelectedIndex = cmbCountry.FindString("Egypt");
            //this will set default department to general medicine.
            cmbDepartment.SelectedIndex = cmbDepartment.FindString("General Medicine");

        }

        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = Country.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cmbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _FillDepartmentsInComoboBox()
        {
            DataTable dtDepartments = Department.GetAllDepartments();

            foreach (DataRow row in dtDepartments.Rows)
            {
                cmbDepartment.Items.Add(row["DepartmentName"]);
            }
        }

        private void _LoadData()
        {
            _Doctor = Doctor.Find(_DoctorID);
            if(_Doctor == null)
            {
                MessageBox.Show("No Doctor with ID = " + _Doctor, "Doctor Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            //IDs
            lblDoctorID.Text = _Doctor.DoctorID.ToString();
            lblPersonID.Text = _Doctor.PersonID.ToString();

            //Text boxes
            txtName.Text = _Doctor.PersonInfo.FullName;
            txtNationalNo.Text = _Doctor.PersonInfo.NationalNo;
            txtLicenseNumber.Text = _Doctor.LicenseNumber;
            txtPhone.Text = _Doctor.PersonInfo.Phone;
            txtEmail.Text = _Doctor.PersonInfo.Email;
            txtAddress.Text = _Doctor.PersonInfo.Address;

            //dates
            dtpBirthDate.Value = _Doctor.PersonInfo.DateOfBirth;
            dtpDateJoined.Value = _Doctor.DateJoined;

            //comboboxes
            cmbCountry.SelectedIndex = cmbCountry.FindString(Country.Find(_Doctor.PersonInfo.NationalityID).CountryName);
            cmbDepartment.SelectedIndex = cmbDepartment.FindString(Department.Find(_Doctor.DepartmentID).DepartmentName);

            //numeric up down
            nupdYearsOfExperience.Value = _Doctor.ExperienceYears;

            //load person image incase it was set.
            if (_Doctor.PersonInfo.PersonPicturePath != "")
            {
                pbImage.ImageLocation = _Doctor.PersonInfo.PersonPicturePath;

            }

            //hide/show the remove linke incase there is no image for the person.
            btnRemoveImage.Visible = (_Doctor.PersonInfo.PersonPicturePath != "");
        }

        private void frmAddEditDoctor_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        //profile picture handling
        private void rbMale_Click(object sender, EventArgs e)
        {
            //change the defualt image to female incase there is no image set.
            if (pbImage.ImageLocation == null)
                pbImage.Image = Resources.doctor_male_pp;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            //change the defualt image to female incase there is no image set.
            if (pbImage.ImageLocation == null)
                pbImage.Image = Resources.doctor_female_pp;
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbImage.Load(selectedFilePath);
                btnRemoveImage.Visible = true;
                // ...
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            pbImage.ImageLocation = null;

            if (rbMale.Checked)
                pbImage.Image = Resources.doctor_male_pp;
            else
                pbImage.Image = Resources.doctor_female_pp;

            btnRemoveImage.Visible = false;
        }

        //validations
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            //Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _Doctor.PersonInfo.NationalNo && Person.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void txtLicenseNumber_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseNumber.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseNumber, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtLicenseNumber, null);
            }

            //Make sure the national number is not used by another person
            if (txtLicenseNumber.Text.Trim() != _Doctor.LicenseNumber && Doctor.IsDoctorExist(txtLicenseNumber.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseNumber, "License Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtLicenseNumber, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            //validate email format
            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            };
        }

        //performing add or edit
        private void FillPersonInfo()
        {
            _Doctor.PersonInfo.NationalityID = Country.Find(cmbCountry.Text).ID; ;
            _Doctor.PersonInfo.FullName = txtName.Text.Trim();
            _Doctor.PersonInfo.NationalNo = txtNationalNo.Text.Trim();
            _Doctor.PersonInfo.Email = txtEmail.Text.Trim();
            _Doctor.PersonInfo.Phone = txtPhone.Text.Trim();
            _Doctor.PersonInfo.Address = txtAddress.Text.Trim();
            _Doctor.PersonInfo.DateOfBirth = dtpBirthDate.Value;

            if (rbMale.Checked)
                _Doctor.PersonInfo.Gender = true;
            else
                _Doctor.PersonInfo.Gender = false;


            if (pbImage.ImageLocation != null)
                _Doctor.PersonInfo.PersonPicturePath = pbImage.ImageLocation;
            else
                _Doctor.PersonInfo.PersonPicturePath = "";

        }

        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_Doctor.PersonInfo.PersonPicturePath != pbImage.ImageLocation)
            {
                if (_Doctor.PersonInfo.PersonPicturePath != "")
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(_Doctor.PersonInfo.PersonPicturePath);
                    }
                    catch (IOException ex)
                    {
                        LoggingManager.LogError(ex);

                    }
                }

                if (pbImage.ImageLocation != null)
                {
                    //then we copy the new image to the image folder after we rename it
                    string SourceImageFile = pbImage.ImageLocation.ToString();

                    if (clsUtility.CopyImageToProjectImagesFolder(ref SourceImageFile, clsUtility.enStaff.Doctor))
                    {
                        pbImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
            {
                return;
            }
            //personal info
            FillPersonInfo();
            //Doctor info
            _Doctor.DateJoined = dtpDateJoined.Value;
            _Doctor.DepartmentID = Department.Find(cmbDepartment.Text).DepartmentID;
            _Doctor.ExperienceYears = Convert.ToByte(nupdYearsOfExperience.Value);
            _Doctor.IsActive = true;
            _Doctor.LicenseNumber = txtLicenseNumber.Text;

            if(_Doctor.Save())
            {
                lblPersonID.Text = _Doctor.PersonID.ToString();
                lblDoctorID.Text = _Doctor.DoctorID.ToString();
                _DoctorID = _Doctor.DoctorID;

                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update Doctor";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Doctor.DoctorID);

                if(_Mode == enMode.AddNew)
                    AddUser();
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void AddUser()
        {
            if (MessageBox.Show("Do you want to create a new user for this person?", "Create new user", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmAddEditUser frm = new frmAddEditUser(_Doctor.PersonID, frmAddEditUser.enRole.Doctor);
                frm.ShowDialog();
            }    
        }
    }

}
