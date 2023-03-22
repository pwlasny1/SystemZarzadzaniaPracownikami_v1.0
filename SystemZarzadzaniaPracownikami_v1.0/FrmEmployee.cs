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
        public EmployeeDetailDTO details = new EmployeeDetailDTO();
        public bool isUpdate = false;
        string imagepath = "";
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

            if(isUpdate)
            {
                txtName.Text = details.Name;
                txtSurname.Text = details.Surname;  
                txtUserNo.Text=details.UserNo.ToString();
                txtPassword.Text = details.Password;
                chAdmin.Checked = Convert.ToBoolean(details.isAdmin);
                txtAdress.Text = details.Adress;
                //dpBirthday.Value = Convert.ToDateTime(details.BirthDay);
                cmbDepartment.SelectedValue = details.DepartmentID;
                cmbPosition.SelectedValue = details.PositionID;
                txtSalary.Text = details.Salary.ToString();
                imagepath = Application.StartupPath + "\\images\\" + details.ImagePath;
                txtImagePath.Text = imagepath;
                pictureBox1.ImageLocation = imagepath;

                if (!UserStatic.isAdmin)
                {
                    chAdmin.Enabled= false;
                    txtUserNo.Enabled = false;
                    txtSalary.Enabled = false;  
                    cmbDepartment.Enabled = false;  
                    cmbPosition.Enabled = false;    

                }


            }
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
                string Unique = Guid.NewGuid().ToString();  
                fileName +=  Unique + openFileDialog1.SafeFileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("ID pracownika nie może być puste");
            
            else if (txtPassword.Text.Trim() == "")
                MessageBox.Show("Hasło nie może być puste");
            else if (txtName.Name.Trim() == "")
                MessageBox.Show("Imię nie może być puste");
            else if (txtSurname.Text.Trim() == "")
                MessageBox.Show("Nazwisko nie może być puste");
            else if (txtImagePath.Text.Trim() == "")
                MessageBox.Show("Dodaj zdjęcie");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Wynagrodzenie nie może być puste");
            else if (cmbDepartment.SelectedIndex == -1)
                MessageBox.Show("Departament nie może być pusty");
            else if (cmbPosition.SelectedIndex == -1)
                MessageBox.Show("Pozycja nie może być pusta");
            
            
            else
            {
                if (!isUpdate)
                {
                    if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text)))
                        MessageBox.Show("ID pracownika jest już używane");
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
                        MessageBox.Show("Pracownik został dodany!");
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
                else
                {
                    DialogResult result = MessageBox.Show("Jesteś pewien?", "Warning", MessageBoxButtons.YesNo);
                    if(result == DialogResult.Yes)
                    {
                        Employee employee= new Employee();
                        if(txtImagePath.Text!=imagepath)
                        {
                            if (File.Exists(@"images\\" + details.ImagePath))
                                File.Delete(@"images\\" + details.ImagePath);
                            File.Copy(txtImagePath.Text, @"images\\" + fileName);
                            employee.ImagePath = fileName;
                        }
                        else
                        {
                            employee.ImagePath= details.ImagePath;
                            employee.ID = details.EmployeeID;
                            employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                            employee.Name = txtName.Text;
                            employee.Surname = txtSurname.Text;
                            employee.isAdmin = chAdmin.Checked;
                            employee.Password = txtPassword.Text;
                            employee.Adress = txtAdress.Text; 
                            employee.BirthDay = dpBirthday.Value;
                            employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                            employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                            employee.Salary = Convert.ToInt32(txtSalary.Text);
                            EmployeeBLL.UpdateEmployee(employee);
                            MessageBox.Show("Dane zostały zaktualizowane");
                            this.Close();

                        }
                    }
                }
                
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
