using HMSBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMS_DesktopApp.Users
{
    public partial class frmAddEditUser : Form
    {
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

        private enRole _role;
        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _PersonID = -1;
        private int _UserID = -1;
        User _User;

        public frmAddEditUser(int PersonID, enRole role)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _role = role;
            this.StartPosition = FormStartPosition.CenterScreen;
            _Mode = enMode.AddNew;
        }

        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _role = enRole.Admin;
            this.StartPosition = FormStartPosition.CenterScreen;
            _Mode = enMode.Update;
        }

        private void _ResetDefualtValues()
        {
            //Setting roles combo box 
            cmbRole.DataSource = Enum.GetValues(typeof(enRole));
            cmbRole.SelectedIndex = (int)_role - 1;
            cmbRole.Enabled = false;

            lblPersonID.Text = _PersonID.ToString();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                _User = new User();
            }
            else
            {
                lblTitle.Text = "Update User";
            }
        }

        private void _LoadData()
        {
            _User = User.Find(_UserID);
            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            lblPersonID.Text = _User.PersonID.ToString();
            txtUserName.Text = _User.UserName.ToString();
            cmbRole.SelectedIndex = _User.RoleID;
            chkIsActive.Checked = _User.IsActive;
        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _User.PersonID = Convert.ToInt32(lblPersonID.Text);
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;
            _User.RoleID = Convert.ToInt32(cmbRole.SelectedIndex + 1);

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;

                lblTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };

        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            };


            if (_Mode == enMode.AddNew)
            {

                if (User.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                };
            }
            else
            {
                //incase update make sure not to use anothers user name
                if (_User.UserName != txtUserName.Text.Trim())
                {
                    if (User.IsUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    };
                }
            }
        }

    }
}
