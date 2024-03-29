﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using DAL;
using BLL;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public partial class FrmPresenceList : Form
    {
        public FrmPresenceList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmPresence frm = new FrmPresence();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(details.PresenceID == 0)
                MessageBox.Show("Wybierz obiekt do edytowania");
            else
            {
            FrmPresence frm = new FrmPresence();
                frm.isUpdate = true;
                frm.details = details;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();

            }
            
        }
        PresenceDTO dto = new PresenceDTO();
        private bool combofull;
        void FillAllData()
        {
            dto = PresenceBLL.GetAll();
            if (!UserStatic.isAdmin)
            {
               dto.Presences = dto.Presences.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            }
            dataGridView1.DataSource = dto.Presences;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
        }

        private void FrmPresenceList_Load(object sender, EventArgs e)
        {
            FillAllData();   
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "ID pracownika";
            dataGridView1.Columns[2].HeaderText = "Imię";
            dataGridView1.Columns[3].HeaderText = "Nazwisko";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].HeaderText = "Początek";
            dataGridView1.Columns[9].HeaderText = "Koniec";
            dataGridView1.Columns[10].HeaderText = "Ilość godzin";
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            if (!UserStatic.isAdmin)
            {
                pnForAdmin.Visible = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<PresenceDetailDTO> list = dto.Presences;

            if (txtUserNo.Text.Trim() != "")
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == (cmbPosition.SelectedItem as PositionDTO).ID).ToList();
            if (rbStartDate.Checked)
                list = list.Where(x => x.StartDate < Convert.ToDateTime(dpFinish.Value) &&
                x.StartDate > Convert.ToDateTime(dpStart.Value)).ToList();
            else if (rbEndDate.Checked)
                list = list.Where(x => x.EndDate < Convert.ToDateTime(dpFinish.Value) &&
                x.EndDate > Convert.ToDateTime(dpStart.Value)).ToList();
            

            dataGridView1.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }

        private void CleanFilters()
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            rbStartDate.Checked = false;            
            rbEndDate.Checked = false;           
            dataGridView1.DataSource = dto.Presences;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Jesteś pewien?", "Warning", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                PresenceBLL.DeletePresence(details.PresenceID);
                MessageBox.Show("Usunięto");
                FillAllData();
                CleanFilters();
            }
        }

        PresenceDetailDTO details = new PresenceDetailDTO();    
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            details.PresenceID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);    
            details.StartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
            details.EndDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            details.Explanation = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
            details.UserNo =  Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            details.PresenceAmount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[10].Value);



        }
    }
}
