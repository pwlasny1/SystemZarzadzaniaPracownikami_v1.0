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
using BLL;
using DAL.DTO;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmPosition : Form
    {
        public FrmPosition()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<Department> departmentlist = new List<Department>();
        public PositionDTO details = new PositionDTO();
        public bool isUpdate = false;

        private void FrmPosition_Load(object sender, EventArgs e)
        {
            departmentlist = DepartmentBLL.GetDepartments();
            cmbDepartment.DataSource = departmentlist; 
            cmbDepartment.DisplayMember= "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            if(isUpdate)
            {
                txtPosition.Text = details.PositionName;
                cmbDepartment.SelectedValue= details.DepartmentID;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtPosition.Text.Length == 0) {
                MessageBox.Show("Please fill the position name");
            }else if(cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("please select department");
            }else
            {
                if(!isUpdate)
                {
                    Position position = new Position();
                    position.PositionName = txtPosition.Text;
                    position.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    BLL.PositionBLL.AddPosition(position);
                    MessageBox.Show("Position was added");
                    txtPosition.Clear();
                    cmbDepartment.SelectedIndex = -1;
                }
                else
                {
                    Position position= new Position();
                    position.ID = details.ID;
                    position.PositionName = txtPosition.Text;
                    position.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    bool control = false;
                    if(Convert.ToInt32(cmbDepartment.SelectedValue) != details.OldDeparmentID)
                        control = true;
                    PositionBLL.UpdatePosition(position, control);
                    MessageBox.Show("Pomyślnie zaktualizowano");
                    this.Close();


                }
            }
        }
    }
}
