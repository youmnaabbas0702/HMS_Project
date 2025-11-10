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
    public partial class frmUsersList : Form
    {
        List<User> _allUsers = User.GetAllUsersList();
        List<User> _ViewedUsersList = new List<User>();

        private enum enUserFilter
        {
            None,
            PersonID,
            UserID,
            UserName,
            Role,
            IsActive
        }
        private enUserFilter _FilterColumn;

        public enum enRole
        {
            All,
            Admin,
            Doctor,
            Nurse,
            Receptionist,
            Pharmacist,
            HR,
            LabTechnician
        }
        private enRole _RoleFilter;

        private enum enIsActiveFilter
        {
            No,
            Yes,
            All
        }
        private enIsActiveFilter _IsActiveFilter;

        public frmUsersList()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(550, 540);

        }

        private void _UpdateCountLabel()
        {
            lblRecordsCount.Text = dgvUsers.RowCount.ToString();
        }

        private void _ReloadUsers()
        {
            _allUsers = User.GetAllUsersList();
            _ViewedUsersList = _allUsers;
        }
        private void _ResetUsersList()
        {
            _ViewedUsersList = _allUsers;
        }

        private void _FormatColumnsView()
        {
            //columns formatting
            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "Person ID";
                dgvUsers.Columns[0].Width = 80;

                dgvUsers.Columns[1].HeaderText = "User ID";
                dgvUsers.Columns[1].Width = 80;

                dgvUsers.Columns[3].HeaderText = "Role";
                dgvUsers.Columns[3].Width = 80;

                dgvUsers.Columns[4].HeaderText = "Is active";
                dgvUsers.Columns[4].Width = 70;

            }
        }

        private void _LoadUsersData()
        {
            var displayList = _ViewedUsersList.Select(u => new
            {
                u.PersonID,
                u.UserID,
                u.UserName,
                Role = ((enRole)u.RoleID).ToString(),
                IsActive = u.IsActive ? "Yes" : "No"
            }).ToList();

            dgvUsers.DataSource = null;
            dgvUsers.DataSource = displayList;

            _FormatColumnsView();

            _UpdateCountLabel();

        }

        private void frmUsersList_Load(object sender, EventArgs e)
        {
            //Data grid view initializing
            _ResetUsersList();
            _LoadUsersData();

            //filter initializing
            cmbFilter.DataSource = Enum.GetValues(typeof(enUserFilter));
            cmbFilter.SelectedIndex = 0;
            cmbRoleFilter.SelectedIndex = 0;
            cmbIsActiveFilter.SelectedIndex = 2;

            txtFilter.Visible = false;
            cmbIsActiveFilter.Visible = false;
            cmbRoleFilter.Visible = false;
        }
       
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _FilterColumn = (enUserFilter)cmbFilter.SelectedIndex;
            if (_FilterColumn == enUserFilter.None)
            {
                _ResetUsersList();
                _LoadUsersData();
                txtFilter.Visible = false;
                cmbIsActiveFilter.Visible = false;
                return;
            }

            switch (_FilterColumn)
            {
                case enUserFilter.PersonID:
                case enUserFilter.UserID:
                case enUserFilter.UserName:
                    txtFilter.Visible = true;
                    break;
                default:
                    txtFilter.Visible = false;
                    break;
            }

            if (_FilterColumn == enUserFilter.IsActive)
                cmbIsActiveFilter.Visible = true;
            else cmbIsActiveFilter.Visible = false;

            if (_FilterColumn == enUserFilter.Role)
                cmbRoleFilter.Visible = true;
            else cmbRoleFilter.Visible = false;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtFilter.Text.Trim().ToLower();
            if (String.IsNullOrEmpty(txtFilter.Text))
            {
                _ResetUsersList();
                _LoadUsersData();
                return;
            }

            switch (_FilterColumn)
            {
                case enUserFilter.None:
                    break;
                case enUserFilter.PersonID:
                    if (int.TryParse(searchText, out int PersonID))
                        _ViewedUsersList = _allUsers.Where(u => u.PersonID == PersonID).ToList();
                    break;
                case enUserFilter.UserID:
                    if (int.TryParse(searchText, out int UserID))
                        _ViewedUsersList = _allUsers.Where(u => u.UserID == UserID).ToList();
                    break;
                case enUserFilter.UserName:
                    _ViewedUsersList = _allUsers.Where(u => u.UserName.ToLower().Contains(searchText)).ToList();
                    break;
                default:
                    break;
            }

            _LoadUsersData();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (_FilterColumn)
            {
                case enUserFilter.PersonID:
                case enUserFilter.UserID:
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true; // prevent the character from being entered
                    }
                    break;
                default:
                    break;
            }
        }
        
        private void cmbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _IsActiveFilter = (enIsActiveFilter)cmbIsActiveFilter.SelectedIndex;
            switch (_IsActiveFilter)
            {
                case enIsActiveFilter.No:
                case enIsActiveFilter.Yes:
                    _ViewedUsersList = _allUsers.Where(u => u.IsActive == (_IsActiveFilter == enIsActiveFilter.Yes)).ToList();

                    break;
                case enIsActiveFilter.All:
                    _ResetUsersList();

                    break;
                default:
                    break;
            }

            _LoadUsersData();
        }

        private void cmbRoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RoleFilter = (enRole)cmbRoleFilter.SelectedIndex;

            switch (_RoleFilter)
            {
                case enRole.Admin:
                case enRole.Doctor:
                case enRole.Nurse:
                case enRole.Receptionist:
                case enRole.Pharmacist:
                case enRole.HR:
                case enRole.LabTechnician:
                    _ViewedUsersList = _allUsers.Where(u => u.RoleID == (int)_RoleFilter).ToList();
                    break;
                case enRole.All:
                default:
                    _ResetUsersList();
                    break;
            }

            _LoadUsersData();
        }


        private void RefreshList()
        {
            _ReloadUsers();
            _LoadUsersData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete User [" + dgvUsers.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                //Perform Delele and refresh
                if (User.DeleteUser((int)dgvUsers.CurrentRow.Cells[1].Value))
                {
                    MessageBox.Show("User Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshList();
                }

                else
                    MessageBox.Show("User was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

    }
}
