﻿using System;
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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmPresence frm = new FrmPresence();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
        }

        private void FrmPresenceList_Load(object sender, EventArgs e)
        {

        }
    }
}
