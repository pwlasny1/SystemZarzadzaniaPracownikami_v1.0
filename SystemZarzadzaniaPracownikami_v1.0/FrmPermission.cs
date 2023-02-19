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
using BLL;


namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmPermission : Form
    {
        public FrmPermission()
        {
            InitializeComponent();
        }

        TimeSpan PermissionDay;
        private void FrmPermission_Load(object sender, EventArgs e)
        {
            txtUserNo.Text = UserStatic.UserNo.ToString();
        }

        private void dpStart_ValueChanged(object sender, EventArgs e)
        {
            PermissionDay = dpFinish.Value.Date - dpStart.Value.Date; 
            txtDayAmount.Text = PermissionDay.TotalDays.ToString();
        }

        private void dpFinish_ValueChanged(object sender, EventArgs e)
        {
            PermissionDay = dpFinish.Value.Date - dpStart.Value.Date;
            txtDayAmount.Text = PermissionDay.TotalDays.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtDayAmount.Text.Trim() == "")
                MessageBox.Show("Wybierz datę początku oraz końca urlopu");
            else if(Convert.ToInt32(txtDayAmount.Text) <= 0)
                MessageBox.Show("Wybierz chociaż jeden dzień");
            else if(txtContent.Text.Trim() == "")
                MessageBox.Show("Podaj opis");
            else
            {
                Permission permission = new Permission();
                permission.EmployeeID = UserStatic.EmployeeID;
                permission.PermissionState = 1;
                permission.PermissionStartDate = dpStart.Value.Date;
                permission.PermissionEndDate= dpFinish.Value.Date;
                permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                permission.PermissionExplanation = txtContent.Text;
                PermissionBLL.AddPermission(permission);
                MessageBox.Show("Permission was added");
                permission = new Permission();
                dpStart.Value = DateTime.Today;
                dpFinish.Value = DateTime.Today;
                txtDayAmount.Clear();
                txtContent.Clear();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
