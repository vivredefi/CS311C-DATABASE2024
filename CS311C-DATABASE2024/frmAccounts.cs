using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311C_DATABASE2024
{
    public partial class frmAccounts : Form
    {
        private string username;
        public frmAccounts(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 accounts = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private void frmAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated FROM tblaccounts WHERE username <> '" + username + "' ORDER BY username");
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
                MessageBox.Show(ex.Message, "Error on account load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated FROM tblaccounts WHERE username <> '" + username + "' AND (username LIKE '%" + txtsearch.Text +
                    "%' OR usertype LIKE '%" + txtsearch.Text + "%') ORDER BY username");
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmAccounts_Load(sender, e);
        }   
        private void btnadd_Click(object sender, EventArgs e)
        {
            frmNewAccount newAccountform = new frmNewAccount(username);
            newAccountform.FormClosed += (s, args) => frmAccounts_Load(sender, e); // Refresh when the add form closes
            newAccountform.ShowDialog();
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    accounts.executeSQL("DELETE FROM tblaccounts WHERE username = '" + selectedUser + "'");
                    if (accounts.rowAffected > 0)
                    {
                        accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Delete', 'Accounts Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Account Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView after deletion
                        frmAccounts_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editusername = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editpassword = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string edittype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editstatus = dataGridView1.Rows[row].Cells[3].Value.ToString();
            frmUpdateAccount updateaccountfrm = new frmUpdateAccount(editusername, editpassword, edittype, editstatus, username);
            updateaccountfrm.FormClosed += (s, args) => frmAccounts_Load(sender, e); // Refresh when the update form closes
            updateaccountfrm.ShowDialog();
        }
    }
}
