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
using DAL.DAO;
using DAL.DTO;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmPositionList : Form
    {
        public FrmPositionList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmPosition frm = new FrmPosition();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(details.ID == 0)
                MessageBox.Show("Wybierz stanowisko");
            else
            {
                FrmPosition frm = new FrmPosition();
                frm.isUpdate= true;
                frm.details = details;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillGrid();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<PositionDTO> positionList = new List<PositionDTO>();   
        void FillGrid()
        {
            positionList = PositionBLL.GetPositions();
            dataGridView1.DataSource = positionList;
        }

        PositionDTO details = new PositionDTO();

        private void FrmPositionList_Load(object sender, EventArgs e)
        {
            FillGrid();
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[0].HeaderText = "Department name";
            dataGridView1.Columns[3].HeaderText = "Position name";
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.PositionName = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            details.ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            details.DepartmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            details.OldDeparmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunąć to stanowisko?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                PositionBLL.DeletePosition(details.ID);
                MessageBox.Show("Usunięto stanowisko");
                FillGrid();
                
            }
        }
    }
}
