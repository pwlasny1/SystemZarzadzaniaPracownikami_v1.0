using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
using DAL.DTO;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmEmployeeList : Form
    {
        public FrmEmployeeList()
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
            FrmEmployee frm= new FrmEmployee();
            this.Hide();
            frm.ShowDialog();
            this.Visible= true;
            FillAllData();
            CleanFilter();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(details.EmployeeID==0)
                MessageBox.Show("Please select employee");
            else
            {
                FrmEmployee frm = new FrmEmployee();
                frm.isUpdate= true;
                frm.details= details;
                this.Hide();
                frm.ShowDialog();
                this.Visible= true; 
                FillAllData();
                CleanFilter();
            }               
        }

        EmployeeDTO dto = new EmployeeDTO();
        private bool combofull = false;
        EmployeeDetailDTO details = new EmployeeDetailDTO();    
        void FillAllData()
        {
            dto = EmployeeBLL.GetAll();
            dataGridView1.DataSource = dto.Employees;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "UserNo";
            dataGridView1.Columns[2].HeaderText = "Name";
            dataGridView1.Columns[3].HeaderText = "Surname";
            dataGridView1.Columns[4].HeaderText = "Department";
            dataGridView1.Columns[5].HeaderText = "Position";
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].HeaderText = "Salary";
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbPosition.SelectedIndex = -1;
            combofull = true;
        }

        private void FrmEmployeeList_Load(object sender, EventArgs e)
        {
                FillAllData();
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<EmployeeDetailDTO> list = dto.Employees;

            if(txtUserNo.Text.Trim() != "")
                list = list.Where(x=> x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if(txtName.Text.Trim() != "")
               list = list.Where(x=> x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x=> x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == (cmbPosition.SelectedItem as PositionDTO).ID).ToList();
                //list = list.Where(x => x.PositionID == ((int)cmbPosition.SelectedValue)).ToList();
            dataGridView1.DataSource= list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilter();
        }

        private void CleanFilter()
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            dataGridView1.DataSource = dto.Employees;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.Name = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            details.Surname = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            details.Password = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            details.ImagePath = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
            details.Adress = dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString();
            details.isAdmin = Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            //details.BirthDay = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[13].Value);
            details.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            details.DepartmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
            details.PositionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
            details.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            details.Salary = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);


        }
    }
}
