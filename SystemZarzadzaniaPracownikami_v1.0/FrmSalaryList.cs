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
using DAL.DAO;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmSalaryList : Form
    {
        public FrmSalaryList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmSalary frm = new FrmSalary();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(details.SalaryID ==0)
                MessageBox.Show("Wybierz wynagrodzenie z listy");
            else
            {
                FrmSalary frm =new FrmSalary(); 
                frm.isUpdate= true;
                frm.details= details;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }
            

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        SalaryDTO dto = new SalaryDTO();
        private bool combofull;

        void FillAllData()
        {
            dto = SalaryBLL.GetAll();
            if (!UserStatic.isAdmin)
                dto.Salaries = dto.Salaries.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dataGridView1.DataSource = dto.Salaries;
            //dataGridView1.DataSource = SalaryDAO.GetSalaries();
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            if (dto.Departments.Count > 0)
                combofull = true;
            cbMonth.DataSource = dto.Months;
            cbMonth.DisplayMember = "MonthName";
            cbMonth.ValueMember = "ID";
            cbMonth.SelectedIndex = -1;
        }

        SalaryDetailDTO details = new SalaryDetailDTO();    

        private void FrmSalaryList_Load(object sender, EventArgs e)
        {
            FillAllData();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "ID pracownika";
            dataGridView1.Columns[2].HeaderText = "Imię";
            dataGridView1.Columns[3].HeaderText = "Nazwisko";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].HeaderText = "Miesiąc";
            dataGridView1.Columns[9].HeaderText = "Rok";
            dataGridView1.Columns[11].HeaderText = "Wynagrodzenie";
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;              
            if (!UserStatic.isAdmin)
            {
                btnUpdate.Hide();
                btnDelete.Hide();
                btnAdd.Hide();
                pnForAdmin.Hide();
                
            }

            

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
            List<SalaryDetailDTO> list = dto.Salaries;

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
            if(txtYear.Text.Trim() != "")
                list = list.Where(x => x.SalaryYear== Convert.ToInt32(txtSalary.Text)).ToList();
            if(cbMonth.SelectedIndex != -1)
                list = list.Where(x => x.MonthID== Convert.ToInt32(cbMonth.SelectedValue)).ToList();
            if(txtSalary.Text.Trim() != "")
            {
                if(rbMore.Checked)
                    list = list.Where(x => x.SalaryAmount > Convert.ToInt32(txtSalary.Text)).ToList();
                else if(rbLess.Checked)
                    list = list.Where(x => x.SalaryAmount < Convert.ToInt32(txtSalary.Text)).ToList();
                else
                    list = list.Where(x => x.SalaryAmount == Convert.ToInt32(txtSalary.Text)).ToList();

            }
           
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
            cbMonth.SelectedIndex = -1;
            rbMore.Checked = false;
            rbLess.Checked = false;
            rbEquals.Checked = false;
            txtSalary.Clear();
            txtYear.Clear();

            dataGridView1.DataSource = dto.Salaries;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.Name = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            details.Surname = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            details.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            details.SalaryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            details.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            details.SalaryYear = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            details.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[10].Value);
            details.SalaryAmount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
            details.OldSalary = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunąć?", "Warning", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                SalaryBLL.DeleteSalary(details.SalaryID);
                MessageBox.Show("Usunięto");
                FillAllData();  
                CleanFilters();
            }
        }

        private void txtExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }
    }
}
