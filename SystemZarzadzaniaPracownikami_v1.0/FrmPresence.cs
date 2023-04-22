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

            dpStart.ValueChanged += dpStart_ValueChanged;
            dpFinish.ValueChanged += dpFinish_ValueChanged;

            if (isUpdate)
            {
                dpStart.Value = details.StartDate;
                dpFinish.Value = details.EndDate;
                txtContent.Text = details.Explanation;
                txtUserNo.Text = details.UserNo.ToString();
            }
            

            hours = dpFinish.Value - dpStart.Value;
            txtHours.Text = hours.TotalHours.ToString();

          
        }

        private void dpStart_ValueChanged(object sender, EventArgs e)
        {
            hours = dpFinish.Value - dpStart.Value;
            txtHours.Text = hours.TotalHours.ToString();
        }

        private void dpFinish_ValueChanged(object sender, EventArgs e)
        {
            hours = dpFinish.Value - dpStart.Value;
            txtHours.Text = hours.TotalHours.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtHours.Text) < 0 || Convert.ToInt32(txtHours.Text) > 12)
            {
                MessageBox.Show("Poprawny zakres godzin to od 1 do 12");
            }
            else
            {
                Presences presence = new Presences();
                if (!isUpdate)
                {
                    presence.EmployeeID = UserStatic.EmployeeID;
                    presence.PresenceStart = dpStart.Value;
                    presence.PresenceEnd = dpFinish.Value;
                    presence.PresenceHours = Convert.ToInt32(txtHours.Text);

                    presence.PresenceExplanation = txtContent.Text;
                    PresenceBLL.AddPresence(presence);
                    MessageBox.Show("Obecność dodana");
                    dpStart.Value = DateTime.Today;
                    dpFinish.Value = DateTime.Today;
                    this.Close();
                }
                else if (isUpdate)
                {
                    DialogResult result = MessageBox.Show("Jesteś pewien?", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        presence.ID = details.PresenceID;
                        presence.PresenceExplanation = txtContent.Text;
                        presence.PresenceStart = dpStart.Value;
                        presence.PresenceEnd = dpFinish.Value;
                        presence.PresenceHours = Convert.ToInt32(txtHours.Text);
                        PresenceBLL.UpdatePresence(presence);
                        MessageBox.Show("Zaktualizowano!");
                        this.Close();
                    }
                }
            }
        }
    }
}
