﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemZarzadzaniaPracownikami_v1._0
{
    public class ExportToExcel
    {
        internal static void ExcelExport(DataGridView dataGridView1)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;

           
            worksheet = workbook.ActiveSheet;

            for(int i = 1; i <+ dataGridView1.Columns.Count; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i -1].HeaderText;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
          
        }
    }
}
