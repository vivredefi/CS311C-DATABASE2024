using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311C_DATABASE2024
{
    public partial class frmUpdateViolation : Form
    {
        private string username, editviolationcode, editdescription, edittype, editstatus;
        Class1 updateviolation = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;

            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Descripiton is empty");
                errorcount++;
            }
            if (cmbviolationtype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbviolationtype, "Select violation type");
                errorcount++;
            }
            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbstatus, "Select status");
                errorcount++;
            }

        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this violation?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updateviolation.executeSQL("UPDATE tblviolations SET description = '" + txtdescription.Text + "', violationtype = '" + cmbviolationtype.Text.ToUpper() + "', status = '" + cmbstatus.Text.ToUpper() +
                            "' WHERE violationcode = '" + txtviolationcode.Text + "'");
                        if (updateviolation.rowAffected > 0)
                        {
                            updateviolation.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Update', 'Violations Management', '" + txtviolationcode.Text + "', '" + username + "')");
                            MessageBox.Show("Violation Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtdescription.Clear();
            cmbstatus.SelectedIndex = -1;
            cmbviolationtype.SelectedIndex = -1;
            txtdescription.Focus();
        }

        private void frmUpdateViolation_Load(object sender, EventArgs e)
        {
            txtviolationcode.Text = editviolationcode;
            txtdescription.Text = editdescription;
            if (edittype == "MINOR OFFENSE")
            {
                cmbviolationtype.SelectedIndex = 0;
            }
            else
            {
                cmbviolationtype.SelectedIndex = 1;
            }
            if (editstatus == "ACTIVE")
            {
                cmbstatus.SelectedIndex = 0;
            }
            else
            {
                cmbstatus.SelectedIndex = 1;
            }
        }

        private int errorcount;
        public frmUpdateViolation(string editviolationcode, string editdescription, string edittype, string editstatus, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editviolationcode = editviolationcode;
            this.editdescription = editdescription;
            this.edittype = edittype;
            this.editstatus = editstatus;
            
        }
    }
}
