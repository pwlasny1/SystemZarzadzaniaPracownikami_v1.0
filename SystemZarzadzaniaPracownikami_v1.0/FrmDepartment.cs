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

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmDepartment : Form
    {
        public FrmDepartment()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDepartment.Text.Length == 0)
                MessageBox.Show("Uzupełnij nazwę departamentu!");
            else
            {
                Department department = new Department();
                if (!isUpdate)
                {
                    department.DepartmentName = txtDepartment.Text;
                    BLL.DepartmentBLL.AddDepartment(department);
                    MessageBox.Show("Departament dodany pomyślnie!");
                    txtDepartment.Clear();
                }
                else
                {
                    DialogResult result = MessageBox.Show("Jesteś pewien?", "Warning!", MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == result) 
                    { 
                        department.ID = details.ID;
                        department.DepartmentName= txtDepartment.Text;
                        DepartmentBLL.UpdateDepartment(department);
                        MessageBox.Show("Pomyślnie zaktualizowano");
                        this.Close();

                    }
                }
            }
        }

        public bool isUpdate = false;
        public Department details = new Department();

        private void FrmDepartment_Load(object sender, EventArgs e)
        {
            if(isUpdate)
            {
                txtDepartment.Text = details.DepartmentName;

            }
        }
    }
}
