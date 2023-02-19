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
using System.IO;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
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

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        EmployeeDTO dto = new EmployeeDTO();
        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dto = EmployeeBLL.GetAll();
            cmbDepartment.DataSource= dto.Departments;
            cmbDepartment.DisplayMember= "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember= "PositionName";
            cmbPosition.ValueMember= "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
        }
        bool combofull = false;
        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {

                int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();

            }
        }

        string fileName = "";
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                txtImagePath.Text = openFileDialog1.FileName;
                
                fileName +=  openFileDialog1.SafeFileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User no is empty");
            else if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text)))
                MessageBox.Show("This UserNo is used by another employee");
            else if (txtPassword.Text.Trim() == "")
                MessageBox.Show("Password is empty");
            else if (txtName.Name.Trim() == "")
                MessageBox.Show("Name is empty");
            else if (txtSurname.Text.Trim() == "")
                MessageBox.Show("Surname is empty");
            else if (txtImagePath.Text.Trim() == "")
                MessageBox.Show("Surname is empty");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Salary is empty");
            else if (cmbDepartment.SelectedIndex == -1)
                MessageBox.Show("Deparment is empty");
            else if (cmbPosition.SelectedIndex == -1)
                MessageBox.Show("Position is empty");
            else if (!File.Exists(@"images\\" + fileName))           
                MessageBox.Show("Image path is not recognized");
            
            else
            {
                Employee employee = new Employee();
                employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                employee.Password = txtPassword.Text;
                employee.isAdmin = chAdmin.Checked;
                employee.Name = txtName.Text;
                employee.Surname = txtSurname.Text;
                employee.Salary = Convert.ToInt32(txtSalary.Text);
                employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                employee.Adress = txtAdress.Text;
                employee.BirthDay = dpBirthday.Value;
                employee.ImagePath = fileName;
                File.Copy(txtImagePath.Text, @"images\\" + fileName);               
                EmployeeBLL.AddEmployee(employee);
                MessageBox.Show("Employee was added");
                txtUserNo.Clear();
                txtPassword.Clear();
                chAdmin.Checked = false;
                txtName.Clear();
                txtSurname.Clear();
                txtSalary.Clear();
                txtAdress.Clear();
                txtImagePath.Clear();
                pictureBox1.Image = null;
                combofull = false;
                cmbDepartment.SelectedIndex = -1;
                cmbPosition.DataSource = dto.Positions;
                cmbPosition.SelectedIndex = -1;
                combofull = true;
                dpBirthday.Value = DateTime.Today;
            }

        }

        bool isUnique = false;
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User no is empty");
            else
            {
                isUnique= EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text));
                if (!isUnique)
                    MessageBox.Show("This UserNo is already exist");
                else
                    MessageBox.Show("You can use this userNo");
            }
        }
    }
}
