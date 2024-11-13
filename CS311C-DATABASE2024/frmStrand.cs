using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311C_DATABASE2024
{
    public partial class frmStrand : Form
    {
        private string username, strandcode;
        
        Class1 strands = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmStrand(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void frmStrand_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = strands.GetData("SELECT strandcode, description, datecreated, createdby FROM tblstrands WHERE strandcode <> '" + strandcode + "' ORDER BY strandcode");
                dataGridView1.DataSource = dt;

                // Set AutoSizeColumnsMode to AllCells to ensure columns fit content
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Optional: Disable the row headers to save space
                dataGridView1.RowHeadersVisible = false;

                // Optional: Disable adding new rows if not needed
                dataGridView1.AllowUserToAddRows = false;

                // Optional: Allow full row selection
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Optional: Disable user resizing of columns
                dataGridView1.AllowUserToResizeColumns = false;

                // Optional: Set default style to make it more readable
                dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.DefaultCellStyle.Padding = new Padding(5);

                // Optional: Set the height of the rows
                dataGridView1.RowTemplate.Height = 30;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on strand load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int row;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cellclick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            frmNewStrand newStrandform = new frmNewStrand(username);
            newStrandform.FormClosed += (s, args) => frmStrand_Load(sender, e); // Refresh when the add form closes
            newStrandform.ShowDialog();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editstrandcode = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editdescription = dataGridView1.Rows[row].Cells[1].Value.ToString();
            frmUpdateStrand updateStrandform = new frmUpdateStrand(editstrandcode, editdescription, username);
            updateStrandform.FormClosed += (s, args) => frmStrand_Load(sender, e); // Refresh when the update form closes
            updateStrandform.ShowDialog();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmStrand_Load(sender, e);
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this Strand?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    strands.executeSQL("DELETE FROM tblstrands WHERE strandcode = '" + selectedUser + "'");
                    if (strands.rowAffected > 0)
                    {
                        strands.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Delete', 'Strands Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Strands Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView after deletion
                        frmStrand_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = strands.GetData("SELECT strandcode, description, datecreated, createdby FROM tblstrands WHERE strandcode <> '" + strandcode + "' AND (strandcode LIKE '%" + txtsearch.Text +
                    "%' OR description LIKE '%" + txtsearch.Text + "%') ORDER BY strandcode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
