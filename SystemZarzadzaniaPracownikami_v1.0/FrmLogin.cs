using BLL;
using DAL;
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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtUserNo.Text.Trim() == "" || txtPassword.Text.Trim()== "")
                MessageBox.Show("Login i hasło są wymagane!");
            else
            {
                List<Employee> employeelist = EmployeeBLL.GetEmployees(Convert.ToInt32(txtUserNo.Text), txtPassword.Text);
                if(employeelist.Count == 0)
                {
                    MessageBox.Show("Dany użytkownik nie istnieje");
                }
                else
                {
                    Employee employee = new Employee();
                    employee = employeelist.First();
                    UserStatic.EmployeeID= employee.ID;
                    UserStatic.UserNo = employee.UserNo;
                    UserStatic.isAdmin = Convert.ToBoolean(employee.isAdmin);
                    FrmMain frm = new FrmMain();
                    this.Hide();
                    frm.ShowDialog();
                }

            }


        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
