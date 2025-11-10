using HMS_DesktopApp.Users;
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
    public partial class frmDoctorsList : Form
    {
        List<Doctor> _allDoctors = Doctor.GetAllDoctorsList();
        List<Doctor> _ViewedDoctorsList = new List<Doctor>();

        private enum enDoctorFilter
        {
            None,           
            PersonID,       
            DoctorID,       
            NationalNo,     
            FullName,       
            CountryName,    
            DateOfBirth,    
            Gender,         
            Address,        
            Phone,          
            Email,
            LicenseNumber,  
            ExperienceYears,
            DepartmentName, 
            DateJoined,     
            IsActive        
        }
        private enDoctorFilter _FilterColumn;

        private enum enGenderFilter
        {
            Female,
            Male,
            All
        }
        private enGenderFilter _GenderFilter;

        private enum enIsActiveFilter
        {
            No,
            Yes,
            All
        }
        private enIsActiveFilter _IsActiveFilter;

        public frmDoctorsList()
        {
            InitializeComponent();

            this.Size = new Size(1070, 510);
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddEditDoctor frm = new frmAddEditDoctor();
            frm.ShowDialog();
            RefreshList();
        }
        private void _UpdateCountLabel()
        {
            lblRecordsCount.Text = dgvDoctors.RowCount.ToString();
        }

        private void _ReloadDoctors()
        {
            _allDoctors = Doctor.GetAllDoctorsList();
            _ViewedDoctorsList = _allDoctors;
        }
        private void _ResetDoctorsList()
        {
            _ViewedDoctorsList = _allDoctors;
        }

        private void _FormatColumnsView()
        {
            //columns formatting
            if (dgvDoctors.Rows.Count > 0)
            {

                dgvDoctors.Columns[0].HeaderText = "Person ID";
                dgvDoctors.Columns[0].Width = 80;

                dgvDoctors.Columns[1].HeaderText = "Doctor ID";
                dgvDoctors.Columns[1].Width = 80;


                dgvDoctors.Columns[2].HeaderText = "National No.";
                dgvDoctors.Columns[2].Width = 90;

                dgvDoctors.Columns[3].HeaderText = "Full Name";
                dgvDoctors.Columns[3].Width = 140;


                dgvDoctors.Columns[4].HeaderText = "Nationality";
                dgvDoctors.Columns[4].Width = 100;

                dgvDoctors.Columns[5].HeaderText = "Date Of Birth";
                dgvDoctors.Columns[5].Width = 100;

                dgvDoctors.Columns[6].HeaderText = "Gender";
                dgvDoctors.Columns[6].Width = 70;

                dgvDoctors.Columns[7].HeaderText = "Address";
                dgvDoctors.Columns[7].Width = 120;


                dgvDoctors.Columns[8].HeaderText = "Phone";
                dgvDoctors.Columns[8].Width = 100;


                dgvDoctors.Columns[9].HeaderText = "Email";
                dgvDoctors.Columns[9].Width = 120;

                dgvDoctors.Columns[10].HeaderText = "License number";
                dgvDoctors.Columns[10].Width = 120;

                dgvDoctors.Columns[11].HeaderText = "Years of experience";
                dgvDoctors.Columns[11].Width = 130;

                dgvDoctors.Columns[12].HeaderText = "Department";
                dgvDoctors.Columns[12].Width = 100;

                dgvDoctors.Columns[13].HeaderText = "Hiring date";
                dgvDoctors.Columns[13].Width = 90;

                dgvDoctors.Columns[14].HeaderText = "Is active";
                dgvDoctors.Columns[14].Width = 70;

            }
        }

        private void _LoadDoctorsData()
        {
            var displayList = _ViewedDoctorsList.Select(d => new
            {
                d.PersonID,
                d.DoctorID,
                d.PersonInfo.NationalNo,
                d.PersonInfo.FullName,
                d.PersonInfo.CountryName,
                d.PersonInfo.DateOfBirth,
                Gender = d.PersonInfo.Gender ? "Male" : "Female",
                d.PersonInfo.Address,
                d.PersonInfo.Phone,
                d.PersonInfo.Email,
                d.LicenseNumber,
                d.ExperienceYears,
                d.DepartmentName,
                d.DateJoined,
                IsActive = d.IsActive ? "Yes" : "No"
            }).ToList();

            dgvDoctors.DataSource = null;
            dgvDoctors.DataSource = displayList;

            _FormatColumnsView();

            _UpdateCountLabel();

        }

        private void frmDoctorsList_Load(object sender, EventArgs e)
        {
            //Data grid view initializing
            _ResetDoctorsList();
            _LoadDoctorsData();

            //filter initializing
            cmbFilter.DataSource = Enum.GetValues(typeof(enDoctorFilter));
            cmbFilter.SelectedIndex = 0;
            cmbGenderFilter.SelectedIndex = 2;
            cmbIsActiveFilter.SelectedIndex = 2;

            txtFilter.Visible = false;
            cmbGenderFilter.Visible = false;
            cmbIsActiveFilter.Visible = false;

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _FilterColumn = (enDoctorFilter)cmbFilter.SelectedIndex;
            if(_FilterColumn == enDoctorFilter.None)
            {
                _ResetDoctorsList();
                _LoadDoctorsData();
                txtFilter.Visible = false;
                cmbIsActiveFilter.Visible = false;
                cmbGenderFilter.Visible = false;
                dtpDateFilter.Visible = false;
                return;
            }

            switch (_FilterColumn)
            {
                case enDoctorFilter.PersonID:
                case enDoctorFilter.DoctorID:
                case enDoctorFilter.NationalNo:
                case enDoctorFilter.FullName:
                case enDoctorFilter.CountryName:
                case enDoctorFilter.Address:
                case enDoctorFilter.Phone:
                case enDoctorFilter.Email:
                case enDoctorFilter.LicenseNumber:
                case enDoctorFilter.ExperienceYears:
                case enDoctorFilter.DepartmentName:
                    txtFilter.Visible = true;
                    break;
                default:
                    txtFilter.Visible = false;
                    break;
            }
            
            if(_FilterColumn == enDoctorFilter.DateOfBirth || _FilterColumn == enDoctorFilter.DateJoined)
                dtpDateFilter.Visible = true;
            else dtpDateFilter.Visible = false;

            if(_FilterColumn == enDoctorFilter.Gender)
                cmbGenderFilter.Visible = true;
            else cmbGenderFilter.Visible = false;

            if(_FilterColumn == enDoctorFilter.IsActive)
                cmbIsActiveFilter.Visible = true;
            else cmbIsActiveFilter.Visible = false;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtFilter.Text.Trim().ToLower();
            if (String.IsNullOrEmpty(txtFilter.Text))
            {
                _ResetDoctorsList();
                _LoadDoctorsData();
                return;
            }

            switch (_FilterColumn)
            {
                case enDoctorFilter.None:
                    break;
                case enDoctorFilter.PersonID:
                    if(int.TryParse(searchText, out int PersonID))
                        _ViewedDoctorsList = _allDoctors.Where(d => d.PersonID == PersonID).ToList();
                    break;
                case enDoctorFilter.DoctorID:
                    if (int.TryParse(searchText, out int DoctorID))
                        _ViewedDoctorsList = _allDoctors.Where(d => d.DoctorID == DoctorID).ToList();
                    break;
                case enDoctorFilter.NationalNo:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.NationalNo.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.FullName:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.FullName.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.CountryName:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.CountryName.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.Address:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.Address.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.Phone:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.Phone.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.Email:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.Email.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.LicenseNumber:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.LicenseNumber.ToLower().Contains(searchText)).ToList();
                    break;
                case enDoctorFilter.ExperienceYears:
                    if (int.TryParse(searchText, out int ExperienceYears))
                        _ViewedDoctorsList = _allDoctors.Where(d => d.ExperienceYears == (byte)ExperienceYears).ToList();
                    break;
                case enDoctorFilter.DepartmentName:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.DepartmentName.ToLower().Contains(searchText)).ToList();
                    break;
                default:
                    break;
            }

            _LoadDoctorsData();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (_FilterColumn)
            {
                case enDoctorFilter.PersonID:
                case enDoctorFilter.DoctorID:
                case enDoctorFilter.ExperienceYears:
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true; // prevent the character from being entered
                    }
                    break;
                default:
                    break;
            }
        }

        private void dtpDateFilter_ValueChanged(object sender, EventArgs e)
        {
            var target = dtpDateFilter.Value.Date;
           
            switch (_FilterColumn)
            {
                case enDoctorFilter.DateOfBirth:
                    _ViewedDoctorsList = _allDoctors
               .Where(d => d.PersonInfo.DateOfBirth.Date == target)
               .ToList();
                    break;
                case enDoctorFilter.DateJoined:
                    _ViewedDoctorsList = _allDoctors
               .Where(d => d.DateJoined.Date == target)
               .ToList();
                    break;
                default:
                    break;
            }

            _LoadDoctorsData();
        }

        private void cmbGenderFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _GenderFilter = (enGenderFilter)cmbGenderFilter.SelectedIndex;
            switch (_GenderFilter)
            {
                case enGenderFilter.Female:
                case enGenderFilter.Male:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.PersonInfo.Gender == (_GenderFilter == enGenderFilter.Male)).ToList();
                    break;
                case enGenderFilter.All:
                    _ResetDoctorsList();
                    break;
                default:
                    break;
            }

            _LoadDoctorsData();

        }

        private void cmbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _IsActiveFilter = (enIsActiveFilter)cmbIsActiveFilter.SelectedIndex;
            switch (_IsActiveFilter)
            {
                case enIsActiveFilter.No:
                case enIsActiveFilter.Yes:
                    _ViewedDoctorsList = _allDoctors.Where(d => d.IsActive == (_IsActiveFilter == enIsActiveFilter.Yes)).ToList();

                    break;
                case enIsActiveFilter.All:
                    _ResetDoctorsList();

                    break;
                default:
                    break;
            }

            _LoadDoctorsData();
        }

        private void RefreshList()
        {
            _ReloadDoctors();
            _LoadDoctorsData();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditDoctor((int)dgvDoctors.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshList();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Doctor [" + dgvDoctors.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                //Perform Delele and refresh
                if (Doctor.DeleteByPersonID((int)dgvDoctors.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Doctor Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshList();
                }

                else
                    MessageBox.Show("Doctor was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void makeUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            if (User.IsPersonAUser(PersonID))
            {
                MessageBox.Show("Person already is a user!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            frmAddEditUser frm = new frmAddEditUser(PersonID, frmAddEditUser.enRole.Doctor);
            frm.ShowDialog();
        }
    }
}
