using HMS_DesktopApp.Doctors;
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
    public partial class frmUserDetails : Form
    {
        public frmUserDetails(int UserID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(770, 620);
            ctrlUserCard1.LoadUserInfo(UserID);
        }
    }
}
