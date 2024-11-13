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
    public partial class frmUpdateStrand : Form
    {
        private string username, editstrandcode, editdescription;
        private int errorcount;
        Class1 updatestrand = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private void frmUpdateStrand_Load(object sender, EventArgs e)
        {
            txtstrandcode.Text = editstrandcode;
            txtdescription.Text = editdescription;
        }

        public frmUpdateStrand(string editstrand, string editdescription, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editstrandcode = editstrand;
            this.editdescription = editdescription;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this strand?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updatestrand.executeSQL("UPDATE tblstrands SET description = '" + txtdescription.Text + "' WHERE strandcode = '" + txtstrandcode.Text + "'");
                        if (updatestrand.rowAffected > 0)
                        {
                            updatestrand.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Update', 'Strands Management', '" + txtstrandcode.Text + "', '" + username + "')");
                            MessageBox.Show("Strand Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Description is empty");
                errorcount++;
            }
        }
        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtdescription.Clear();
            txtdescription.Focus();
        }
    }
}
