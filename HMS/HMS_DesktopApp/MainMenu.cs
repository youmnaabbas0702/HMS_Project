using HMS_DesktopApp.Doctors;
using HMS_DesktopApp.Global_Classes;
using HMS_DesktopApp.Users;
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
    public partial class MainMenu : Form
    {
        frmLogin _frmLogin;

        public MainMenu(frmLogin frm)
        {
            InitializeComponent();
            _frmLogin = frm;
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;

        }

        private void doctorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoctorsList frm = new frmDoctorsList();
            frm.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsersList frm = new frmUsersList();
            frm.ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginManager.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }
    }
}
