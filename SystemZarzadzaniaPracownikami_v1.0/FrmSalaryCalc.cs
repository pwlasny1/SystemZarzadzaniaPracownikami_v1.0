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
    public partial class FrmSalaryCalc : Form
    {
        public FrmSalaryCalc()
        {
            InitializeComponent();
        }

        private void FrmSalaryCalc_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSalary.Text = "";
            txtHours.Text = "";
            txtAmount.Text = "";

            rbFreeDay.Checked = false;
            rbWorkDay.Checked = false;  
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            int salary = Convert.ToInt32(txtSalary.Text);
            int hours = Convert.ToInt32(txtHours.Text);
            int amount = 0;

            if (rbFreeDay.Checked)
            {
                // Obliczanie kwoty dodatkowej wypłaty za nadgodziny w dni wolne
                amount = salary / 160 * hours * 2;
            }
            else if (rbWorkDay.Checked)
            {
                amount = (int)(salary / 160 * hours * 1.5);
                // Obliczanie kwoty dodatkowej wypłaty za nadgodziny w dni robocze
            }

            // Wyświetlenie kwoty dodatkowej wypłaty
            txtAmount.Text = amount.ToString();
        }
    }
}
