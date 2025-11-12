using Common;
using HMS_DesktopApp.Global_Classes;
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

namespace HMS_DesktopApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (LoginManager.LoadCredentials(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim(), password = txtPassword.Text.Trim();
            if (User.CheckUserCredentials(username, password))
            {
                User user = User.Find(username);

                if (chkRememberMe.Checked)
                {
                    //store username and password
                    LoginManager.SaveCredentials(username, password);

                }
                else
                {
                    //store empty username and password
                    LoginManager.SaveCredentials("", "");

                }

                //incase the user is not active
                if (!user.IsActive)
                {

                    txtUserName.Focus();
                    MessageBox.Show("Your account is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LoginManager.CurrentUser = user;
                this.Hide();
                MainMenu frm = new MainMenu(this);
                frm.ShowDialog();


            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RequiredField_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                epRequired.SetError((TextBox)sender, "This field is required");
            }
            else
                epRequired.SetError((TextBox)sender, "");
        }
    }
}
