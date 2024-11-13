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
    public partial class frmNewViolation : Form
    {
        private string username;
        private int errorcount;
        Class1 newviolation = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmNewViolation(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtviolationcode.Text))
            {
                errorProvider1.SetError(txtviolationcode, "Violation Code is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Description is empty");
                errorcount++;
            }
            if (cmbviolationtype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbviolationtype, "Select violation type");
                errorcount++;
            }
            try
            {
                DataTable dt = newviolation.GetData("SELECT * FROM tblviolations WHERE violationcode = '" + txtviolationcode.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtviolationcode, "Violation Code is already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating exisiting violation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this violation?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newviolation.executeSQL("INSERT INTO tblviolations (violationcode, description, violationtype, status, createdby, datecreated) VALUES ('" + txtviolationcode.Text + "', '" + txtdescription.Text + "', '" + cmbviolationtype.Text.ToUpper() + "', 'ACTIVE', '" +
                            username + "', '" + DateTime.Now.ToShortDateString() + "')");
                        if (newviolation.rowAffected > 0)
                        {
                            newviolation.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'Add', 'Violations Management', '" + txtviolationcode.Text + "', '" + username + "')");

                            MessageBox.Show("New Violation Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtviolationcode.Clear();
            txtdescription.Clear();
            cmbviolationtype.SelectedIndex = -1;
            txtviolationcode.Focus();
        }
    }
}
