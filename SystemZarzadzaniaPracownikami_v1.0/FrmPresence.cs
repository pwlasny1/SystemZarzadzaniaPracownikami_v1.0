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
    public partial class FrmPresence : Form
    {
        public FrmPresence()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        TimeSpan hours;
        public bool isUpdate = false;

        private void FrmPresence_Load(object sender, EventArgs e)
        {
            txtUserNo.Text = UserStatic.UserNo.ToString();

            dpStart.Format = DateTimePickerFormat.Custom;
            dpStart.CustomFormat = "HH:mm";
            dpStart.ShowUpDown = true;

            dpFinish.Format = DateTimePickerFormat.Custom;
            dpFinish.CustomFormat = "HH:mm";
            dpFinish.ShowUpDown = true;

            hours = dpFinish.Value - dpStart.Value;
            txtDayAmount.Text = hours.TotalHours.ToString();
        }

        private void dpStart_ValueChanged(object sender, EventArgs e)
        {
            hours = dpFinish.Value - dpStart.Value;
            txtDayAmount.Text = hours.TotalHours.ToString();
        }

        private void dpFinish_ValueChanged(object sender, EventArgs e)
        {
            hours = dpFinish.Value - dpStart.Value;
            txtDayAmount.Text = hours.TotalHours.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Presence presence = new Presence();
            presence.EmployeeID = UserStatic.EmployeeID;
            presence.PresenceStartDate = dpStart.Value; // użyj Value zamiast Value.Date
            presence.PresenceEndDate = dpFinish.Value; // użyj Value zamiast Value.Date
            presence.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
            
            presence.PresenceExplanation = txtContent.Text;
            PresenceBLL.AddPresence(presence);
            MessageBox.Show("Obecność dodana");
            dpStart.Value = DateTime.Now;
            dpFinish.Value = DateTime.Now ;
            this.Close();
        }
    }
}
