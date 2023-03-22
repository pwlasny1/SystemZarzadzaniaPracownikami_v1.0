using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DAL.DTO;
using BLL;




namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmTaskList : Form
    {
        public FrmTaskList()
        {
            InitializeComponent();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmTask frm = new FrmTask();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (details.TaskID == 0)
                MessageBox.Show("Please select a task on table");
            else
            {
                FrmTask frm = new FrmTask();
                frm.isUpdate= true; 
                frm.details = details;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();

            }
        }

        TaskDTO dto = new TaskDTO();
        private bool combofull = false;


        void FillAllData()
        {
            dto = TaskBLL.GetAll();
            if (!UserStatic.isAdmin)
            {
                dto.Tasks = dto.Tasks.Where(x=> x.EmployeeID == UserStatic.EmployeeID).ToList();
            }
            dataGridView1.DataSource = dto.Tasks;
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
            cmbTaskState.DataSource = dto.TaskStates;
            cmbTaskState.DisplayMember = "StateName";
            cmbTaskState.ValueMember = "ID";
            cmbTaskState.SelectedIndex = -1;
        }

        TaskDetailDTO details = new TaskDetailDTO();
        private void FrmTaskList_Load(object sender, EventArgs e)
        {

            FillAllData();
           
            dataGridView1.Columns[0].HeaderText = "Tytuł";
            dataGridView1.Columns[1].HeaderText = "Identyfikator";
            dataGridView1.Columns[2].HeaderText = "Imię";
            dataGridView1.Columns[3].HeaderText = "Nazwisko";
            dataGridView1.Columns[4].HeaderText = "Data nadania";
            dataGridView1.Columns[5].HeaderText = "Data ukończenia";
            dataGridView1.Columns[6].HeaderText = "Status";
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            if (!UserStatic.isAdmin)
            {
                btnAdd.Enabled = false; 
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                pnForAdmin.Hide();
                btnApprove.Text = "Wykonano";


            }
           
        }


        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource = list.Where(x => x.DepartmentID ==
                Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;

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
            if(rbStartDate.Checked)
                list = list.Where(x=> x.TaskStartDate > Convert.ToDateTime(dpStart.Value) && 
                x.TaskStartDate < Convert.ToDateTime(dpFinish.Value)).ToList();
            if (rbDeliveryDate.Checked)
                list = list.Where(x => x.TaskEndDate < Convert.ToDateTime(dpStart.Value) &&
                x.TaskEndDate < Convert.ToDateTime(dpFinish.Value)).ToList();
            if(cmbTaskState.SelectedIndex != -1)
                list = list.Where(x => x.taskStateID== Convert.ToInt32(cmbTaskState.SelectedValue)).ToList();
            
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
            rbDeliveryDate.Checked = false;
            rbStartDate.Checked = false;
            cmbTaskState.SelectedIndex = -1;
            dataGridView1.DataSource = dto.Tasks;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.Name = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            details.Surname = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            details.Title = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            details.Content = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            details.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
           // details.taskStateID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[14].Value);
            details.TaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
            details.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            details.TaskStartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            details.TaskEndDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value);


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Na pewno chcesz usunąć?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                TaskBLL.DeleteTask(details.TaskID);
                MessageBox.Show("Usunięto");
                FillAllData();
                CleanFilters();
            }
        }

        private void txtExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if(UserStatic.isAdmin && details.taskStateID == TaskStates.OnEmployee && details.EmployeeID != UserStatic.EmployeeID )
                MessageBox.Show("Status zadania musi być jako wykonany zanim zaakceptujesz zadanie");
            else if(UserStatic.isAdmin && details.taskStateID == TaskStates.Approved)
                MessageBox.Show("Zadanie zostało już zaakceptowane jako gotowe");
            else if(!UserStatic.isAdmin && details.taskStateID == TaskStates.Delivered)
                MessageBox.Show("Zadanie zostało już wykonane");
            else if(!UserStatic.isAdmin && details.taskStateID == TaskStates.Approved)
                MessageBox.Show("Zadanie zostało już zaakceptowane jako gotowe");
            else
            {
                TaskBLL.ApproveTask(details.TaskID, UserStatic.isAdmin);
                MessageBox.Show("Zaktualizowano status zadania");
                FillAllData();
                CleanFilters();  
            }
        }
    }
}
