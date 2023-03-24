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
using DAL.DTO;

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
        public PresenceDetailDTO details = new PresenceDetailDTO();

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

            if (isUpdate)
            {
                dpStart.Value = details.StartDate;
                dpFinish.Value = details.EndDate;
                txtDayAmount.Text = details.PresenceAmount.ToString();
                txtContent.Text = details.Explanation;
                txtUserNo.Text = details.UserNo.ToString();

            }
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
            if(!isUpdate)
            {
                presence.EmployeeID = UserStatic.EmployeeID;
                presence.PresenceStartDate = dpStart.Value; // użyj Value zamiast Value.Date
                presence.PresenceEndDate = dpFinish.Value; // użyj Value zamiast Value.Date
                presence.PermissionDay = Convert.ToInt32(txtDayAmount.Text);

                presence.PresenceExplanation = txtContent.Text;
                PresenceBLL.AddPresence(presence);
                MessageBox.Show("Obecność dodana");
                dpStart.Value = DateTime.Today;
                dpFinish.Value = DateTime.Today;
                this.Close();
            } else if(isUpdate)
            {
                DialogResult result = MessageBox.Show("Jesteś pewien?", "Warning", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    presence.ID = details.PresenceID;
                    presence.PresenceExplanation = txtContent.Text;
                    presence.PresenceStartDate = dpStart.Value;
                    presence.PresenceEndDate= dpFinish.Value;
                    presence.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                    PresenceBLL.UpdatePresence(presence);
                    MessageBox.Show("Zaktualizowano!");
                    this.Close();
                }
            }
        }
    }
}
