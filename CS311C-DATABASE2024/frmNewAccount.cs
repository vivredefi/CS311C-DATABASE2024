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
    public partial class frmNewAccount : Form
    {
        private string username;
        private int errorcount;
        public event Action AccountAdded;
        Class1 newaccount = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmNewAccount(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtusername.Text)) 
            {
                errorProvider1.SetError(txtusername, "Username is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Password is empty");
                errorcount++;
            }
            if (txtpassword.TextLength < 6)
            {
                errorProvider1.SetError(txtpassword, "Password must be at least 6 characters");
                errorcount++;
            }
            if (cmbtype.SelectedIndex < 0) 
            {
                errorProvider1.SetError(cmbtype, "Select usertype");
                errorcount++;
            }
            try
            {
                DataTable dt = newaccount.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "'");
                if (dt.Rows.Count > 0) 
                { 
                    errorProvider1.SetError(txtusername, "Username is already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating exisiting account", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0) 
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newaccount.executeSQL("INSERT INTO tblaccounts (username, password, usertype, status, createdby, datecreated) VALUES ('" + txtusername.Text + "', '" + txtpassword.Text + "', '" +
                            cmbtype.Text.ToUpper() + "', 'ACTIVE', '" + username + "', '" + DateTime.Now.ToShortDateString() + "')");
                        if (newaccount.rowAffected > 0)
                        {
                            newaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'Add', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");

                            MessageBox.Show("New account added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void cbshow_CheckedChanged(object sender, EventArgs e)
        {
            if (cbshow.Checked == true)
            {
                txtpassword.PasswordChar = '\0';
            }
            else
            {
                txtpassword.PasswordChar = '*';
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            cmbtype.SelectedIndex = -1;
            txtusername.Focus();
        }

       
    }
}
