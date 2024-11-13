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
    public partial class frmNewStrand : Form
    {
        private string username;
        private int errorcount;
        Class1 newstrand = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmNewStrand(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtstrandcode.Text))
            {
                errorProvider1.SetError(txtstrandcode, "Strand Code is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Description is empty");
                errorcount++;
            }
            try
            {
                DataTable dt = newstrand.GetData("SELECT * FROM tblstrands WHERE strandcode = '" + txtstrandcode.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtstrandcode, "Strand Code is already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating exisiting course", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this strand?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newstrand.executeSQL("INSERT INTO tblstrands (strandcode, description, datecreated, createdby) VALUES ('" + txtstrandcode.Text + "', '" + txtdescription.Text + "', '" +
                            DateTime.Now.ToShortDateString() + "', '" + username + "')");
                        if (newstrand.rowAffected > 0)
                        {
                            newstrand.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'Add', 'Strands Management', '" + txtstrandcode.Text + "', '" + username + "')");

                            MessageBox.Show("New Strand Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtstrandcode.Clear();
            txtdescription.Clear();
            txtstrandcode.Focus();
        }
    }
}
