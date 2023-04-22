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
    public partial class FrmDepartmentList : Form
    {
        public FrmDepartmentList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmDepartment frm = new FrmDepartment();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            departments = DepartmentBLL.GetDepartments();
            dataGridView1.DataSource = departments;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(details.ID == 0)
                MessageBox.Show("Wybierz departament");
            else
            {
                FrmDepartment frm = new FrmDepartment();
                frm.isUpdate= true;
                frm.details = details;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                departments = BLL.DepartmentBLL.GetDepartments();
                dataGridView1.DataSource = departments;
            }
        }

        List<Department> departments = new List<Department>();
        public Department details = new Department();

        private void FrmDepartmentList_Load(object sender, EventArgs e)
        {
            List<Department> departments= new List<Department>();
            departments = BLL.DepartmentBLL.GetDepartments();
            dataGridView1.DataSource= departments;
            dataGridView1.Columns[0].HeaderText = "Numer departamentu";
            dataGridView1.Columns[1].HeaderText = "Nazwa departamentu";

        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);    
            details.DepartmentName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();  
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunąć to stanowisko?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) 
            { 
                DepartmentBLL.DeleteDepartment(details.ID);
                MessageBox.Show("Usunięto departament");
                departments = DepartmentBLL.GetDepartments();
                dataGridView1.DataSource = departments;

            }

        }
    }
}
