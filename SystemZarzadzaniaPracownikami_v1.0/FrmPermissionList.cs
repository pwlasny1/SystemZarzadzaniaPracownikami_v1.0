using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using DAL;
using BLL;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmPermissionList : Form
    {
        public FrmPermissionList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmPermission frm = new FrmPermission();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (details.PermissionID == 0)
                MessageBox.Show("Please select a permission from table");
            else if(details.State ==PermissionStates.Approved || details.State == PermissionStates.Dissaproved)
                MessageBox.Show("W tym statusie nie możesz aktualizować wniosku.");
            else
            {
                FrmPermission frm = new FrmPermission();
                frm.isUpdate= true;
                frm.details= details;
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }
                   
        }

        PermissionDTO dto = new PermissionDTO();
        private bool combofull;
        void FillAllData()
        {

            dto = PermissionBLL.GetAll();
            if (!UserStatic.isAdmin)
            {
                dto.Permissions = dto.Permissions.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();  
            }
            dataGridView1.DataSource = dto.Permissions;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;       
               combofull = true;
            cbState.DataSource = dto.States;
            cbState.DisplayMember = "StateName";
            cbState.ValueMember = "ID";
            cbState.SelectedIndex = -1;
        }

        private void FrmPermissionList_Load(object sender, EventArgs e)
        {
            FillAllData();
           

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "UserNo";
            dataGridView1.Columns[2].HeaderText = "Name";
            dataGridView1.Columns[3].HeaderText = "Surname";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].HeaderText = "Start date";
            dataGridView1.Columns[9].HeaderText = "End date";
            dataGridView1.Columns[10].HeaderText = "Day amount";
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[11].HeaderText = "state";
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            if (!UserStatic.isAdmin)
            {
                pnForAdmin.Visible = false;
                btnApprove.Hide();
                btnDisapprove.Hide();
                btnDelete.Hide();
                btnClose.Location = new Point(776,42);

            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<PermissionDetailDTO> list = dto.Permissions;
            if (txtUserNo.Text.Trim() != "")
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == (cmbPosition.SelectedItem as PositionDTO).ID).ToList();
            if (rbStartDate.Checked)
                list = list.Where(x => x.StartDate < Convert.ToDateTime(dpEnd.Value) &&
                x.StartDate > Convert.ToDateTime(dpStart.Value)).ToList();
            else if(rbEndDate.Checked)
                list = list.Where(x => x.EndDate < Convert.ToDateTime(dpEnd.Value) &&
              x.EndDate > Convert.ToDateTime(dpStart.Value)).ToList();

            if (cbState.SelectedIndex != -1)
                list = list.Where(x => x.State == Convert.ToInt32(cbState.SelectedValue)).ToList();
            if(txtDayAmount.Text.Trim() != "")
                list = list.Where(x => x.PermissionDayCount == Convert.ToInt32(txtDayAmount.Text)).ToList();


            dataGridView1.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }

        private void CleanFilters()
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            rbEndDate.Checked = false;
            rbStartDate.Checked = false;
            cbState.SelectedIndex = -1;
            txtDayAmount.Clear();
            dataGridView1.DataSource = dto.Permissions;
        }

        PermissionDetailDTO details = new PermissionDetailDTO();   
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.PermissionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[14].Value);
            details.StartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
            details.EndDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            details.Explanation = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            details.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            details.State = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            details.PermissionDayCount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[10].Value);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(details.PermissionID, PermissionStates.Approved);
            MessageBox.Show("Approved");
            FillAllData();
            CleanFilters();

        }

        private void btnDisapprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(details.PermissionID, PermissionStates.Dissaproved);
            MessageBox.Show("Disapproved");
            FillAllData();
            CleanFilters();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunąć to zdarzenie?", "Warning", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                if(details.State == PermissionStates.Approved)
                    MessageBox.Show("Nie możesz usunąć zaakceptowanego urlopu");
                else
                {
                    PermissionBLL.DeletePermission(details.PermissionID);
                    MessageBox.Show("Usunięto");
                    FillAllData(); 
                    CleanFilters();
                }
            }

        }

        private void txtExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }
    }
}
