using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311C_DATABASE2024
{
    public partial class frmViolations : Form
    {
        private string username, violationID;
        Class1 violations = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmViolations(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = violations.GetData("SELECT violationcode, description, violationtype,  status, createdby, datecreated FROM tblviolations WHERE violationcode <> '" + violationID + "' AND (violationcode LIKE '%" + txtsearch.Text +
                    "%' OR description LIKE '%" + txtsearch.Text + "%' OR violationtype LIKE '%" + txtsearch.Text + "%') ORDER BY violationcode");
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            frmNewViolation newViolationform = new frmNewViolation(username);
            newViolationform.FormClosed += (s, args) => frmViolations_Load(sender, e); // Refresh when the add form closes
            newViolationform.ShowDialog();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmViolations_Load(sender, e);
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editviolationcode = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editdescription = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string edittype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editstatus = dataGridView1.Rows[row].Cells[3].Value.ToString();
            frmUpdateViolation updateViolationform = new frmUpdateViolation(editviolationcode, editdescription, edittype, editstatus, username);
            updateViolationform.FormClosed += (s, args) => frmViolations_Load(sender, e); // Refresh when the update form closes
            updateViolationform.ShowDialog();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this violation?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    violations.executeSQL("DELETE FROM tblviolations WHERE violationcode = '" + selectedUser + "'");
                    if (violations.rowAffected > 0)
                    {
                        violations.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Delete', 'Violations Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Violation Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView after deletion
                        frmViolations_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmViolations_Load(object sender, EventArgs e)
        {
            DataTable dt = violations.GetData("SELECT violationcode, description, violationtype, status, createdby, datecreated FROM tblviolations WHERE violationcode <> '" + violationID + "' ORDER BY violationcode");
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
    }
}
