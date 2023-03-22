using BLL;
using DAL;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmSalary : Form
    {
        public FrmSalary()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        SalaryDTO dto = new SalaryDTO();
        private bool combofull = false;
        public SalaryDetailDTO details = new SalaryDetailDTO(); 
        public bool isUpdate = false;


        private void FrmSalary_Load(object sender, EventArgs e)
        {
            dto = SalaryBLL.GetAll();

            if(!isUpdate)
            {
                dataGridView1.DataSource = dto.Employees;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "ID Użytkownika";
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

                if (dto.Departments.Count > 0)
                    combofull = true;
            }

            cbMonth.DataSource = dto.Months;
            cbMonth.DisplayMember = "MonthName";
            cbMonth.ValueMember = "ID";
            cbMonth.SelectedIndex = -1;

            if (isUpdate)
            {
                panel1.Hide();
                txtName.Text = details.Name;
                txtSalary.Text = details.SalaryAmount.ToString();
                txtSurname.Text = details.Surname;
                txtYear.Text = details.SalaryYear.ToString();
                cbMonth.SelectedValue = details.MonthID;

            }

     
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
             if (txtYear.Text.Trim() == "")
                MessageBox.Show("Uzupełnij rok");
            else if (cbMonth.SelectedIndex == -1)
                MessageBox.Show("Uzupełnij miesiąc");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Upzupełnij wynagrodzenie");
            
            else
            {
                bool control = false;
                if (!isUpdate)
                {
                    if (salary.EmployeeID == 0)
                        MessageBox.Show("Wybierz pracownika");
                    else
                    {
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthID = Convert.ToInt32(cbMonth.SelectedValue);
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        if(salary.Amount> oldsalary)
                            control = true;
                        SalaryBLL.AddSalary(salary,control);
                        MessageBox.Show("Dodano pomyślnie");
                        cbMonth.SelectedIndex = -1;
                        salary = new Salary();
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Jesteś pewien?", "title", MessageBoxButtons.YesNo);
                    if(DialogResult.Yes == result)
                    {
                        Salary salary = new Salary();
                        salary.ID = details.SalaryID;
                        salary.EmployeeID = details.EmployeeID;
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthID = Convert.ToInt32(cbMonth.SelectedValue);
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        
                        if(salary.Amount > details.OldSalary)
                        
                            control = true;

                        SalaryBLL.UpdateSalary(salary,control);
                        MessageBox.Show("Umowa zaktualizowana");
                        this.Close();   

                    }
                }

            }
        }

        Salary salary = new Salary();
        int oldsalary = 0;
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtYear.Text = DateTime.Today.Year.ToString();
            txtSalary.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            salary.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            oldsalary = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
        }
    }
}
