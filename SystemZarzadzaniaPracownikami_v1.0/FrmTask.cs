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
using BLL;
using DAL;
using Task = DAL.Task;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmTask : Form
    {
        public FrmTask()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        TaskDTO dto = new TaskDTO();
        private bool combofull = false;
        public bool isUpdate = false;
        public TaskDetailDTO details = new TaskDetailDTO();

        private void FrmTask_Load(object sender, EventArgs e)
        {
            
            dto = TaskBLL.GetAll();
            dataGridView1.DataSource= dto.Employees;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "ID pracownika";
            dataGridView1.Columns[2].HeaderText = "Imię";
            dataGridView1.Columns[3].HeaderText = "Nazwisko";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;

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
            if(isUpdate)
            {
                label9.Visible = true;
                cmbTaskState.Visible = true;
                txtName.Text = details.Name;
                txtUserNo.Text = details.UserNo.ToString();
                txtSurname.Text = details.Surname;
                txtTitle.Text = details.Title;
                txtContent.Text = details.Content;
                cmbTaskState.DataSource = dto.TaskStates;
                cmbTaskState.DisplayMember = "StateName";
                cmbTaskState.ValueMember = "ID";
                cmbTaskState.SelectedIndex = details.taskStateID;

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

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            task.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                
                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource = list.Where(x => x.PositionID ==
                Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
        }

        Task task = new Task();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (task.EmployeeID == 0)
                MessageBox.Show("Wybierz pracownika");
            else if (txtTitle.Text.Trim() == "")
                MessageBox.Show("Tytuł nie może być pusty");
            else if (txtContent.Text.Trim() == "")
                MessageBox.Show("Opis nie może być pusty");
            else
            {
                if (!isUpdate)
                {
                    task.TaskTitle = txtTitle.Text;
                    task.TaskContent = txtContent.Text;
                    task.TaskStartDate = DateTime.Today;
                    task.TaskState = 1;
                    TaskBLL.AddTask(task);
                    MessageBox.Show("Dodano zadanie");

                    txtTitle.Clear();
                    txtContent.Clear();
                    task = new Task();

                }
                else if (isUpdate)
                {
                    DialogResult result = MessageBox.Show("Jesteś pewien?", "Warning!!", MessageBoxButtons.YesNo);
                    if(result == DialogResult.Yes)
                    {
                        Task update = new Task();
                        update.ID = details.TaskID;
                        if(Convert.ToInt32(txtUserNo.Text) != details.UserNo)
                        
                            update.EmployeeID= details.EmployeeID;
                        
                        else
                        
                            update.EmployeeID = details.EmployeeID;
                            update.TaskTitle = txtTitle.Text;
                            update.TaskContent = txtContent.Text;
                            update.TaskState = Convert.ToInt32(cmbTaskState.SelectedValue);
                            TaskBLL.UpdateTask(update);
                            MessageBox.Show("Zaktualizowano zadanie");
                            this.Close();
                        
                    }
                }

            }
         
        }
    }
}
