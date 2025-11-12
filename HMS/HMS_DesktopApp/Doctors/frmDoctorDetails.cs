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

namespace HMS_DesktopApp.Doctors
{
    public partial class frmDoctorDetails : Form
    {
        public frmDoctorDetails(int DoctorID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(770, 550);
            ctrlDoctorCard1.LoadDoctorInfo(DoctorID);

        }
    }
}
