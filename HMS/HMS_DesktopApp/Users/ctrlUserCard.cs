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

namespace HMS_DesktopApp.Users
{
    public partial class ctrlUserCard : UserControl
    {

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public enum enRole
        {
            Admin = 1,
            Doctor,
            Nurse,
            Receptionist,
            Pharmacist,
            HR,
            LabTechnician
        }

        private User _User;
        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }

        public User SelectedUserInfo
        {
            get { return _User; }
        }

        public void LoadUserInfo(int userID)
        {
            _User = User.Find(userID); // assuming you have User.Find(id)
            if (_User == null)
            {
                ResetUserInfo();
                MessageBox.Show("No User with ID = " + userID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillUserInfo();
        }

        private void _LoadPersonImage()
        {
            if (_User.PersonInfo.Gender)
                pbImage.Image = Resources.user_male_pp;
            else
                pbImage.Image = Resources.user_female_pp;

            string imagePath = _User.PersonInfo.PersonPicturePath;
            if (!string.IsNullOrEmpty(imagePath))
            {
                if (File.Exists(imagePath))
                    pbImage.ImageLocation = imagePath;
                else
                    MessageBox.Show("Could not find this image: " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _FillUserInfo()
        {
            llEditUserInfo.Enabled = true;
            _UserID = _User.UserID;

            //Person Info
            lblPersonID.Text = _User.PersonInfo.PersonID.ToString();
            lblNationalNo.Text = _User.PersonInfo.NationalNo;
            lblName.Text = _User.PersonInfo.FullName;
            lblGender.Text = _User.PersonInfo.Gender ? "Male" : "Female";
            pbGender.Image = _User.PersonInfo.Gender ? Resources.doctor_male : Resources.doctor_female;
            lblEmail.Text = _User.PersonInfo.Email;
            lblPhone.Text = _User.PersonInfo.Phone;
            lblBirthDate.Text = _User.PersonInfo.DateOfBirth.ToShortDateString();
            lblCountry.Text = Country.Find(_User.PersonInfo.NationalityID).CountryName;
            lblAddress.Text = _User.PersonInfo.Address;

            //User Info
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
            lblRole.Text = ((enRole)_User.RoleID).ToString();

            _LoadPersonImage();
        }

        public void ResetUserInfo()
        {
            _UserID = -1;

            // Person Info
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblBirthDate.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbGender.Image = Resources.doctor_male;
            pbImage.Image = Resources.user_male_pp;

            // User Info
            lblUserID.Text = "[????]";
            lblUserName.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblRole.Text = "[????]";
        }

        private void llEditUserInfo_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser(_UserID);
            frm.ShowDialog();

            // refresh data
            LoadUserInfo(_UserID);
        }
    }
}
