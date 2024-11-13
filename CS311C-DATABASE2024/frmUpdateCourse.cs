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
    public partial class frmUpdateCourse : Form
    {
        private string username, editcoursecode, editdescription;
        private int errorcount;
        
        Class1 updatecourse = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this course?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updatecourse.executeSQL("UPDATE tblcourses SET description = '" + txtdescription.Text  + "' WHERE coursecode = '" + txtcoursecode.Text + "'");
                        if (updatecourse.rowAffected > 0)
                        {
                            updatecourse.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Update', 'Courses Management', '" + txtcoursecode.Text + "', '" + username + "')");
                            MessageBox.Show("Course Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void frmUpdateCourse_Load(object sender, EventArgs e)
        {
            txtcoursecode.Text = editcoursecode;
            txtdescription.Text = editdescription;
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtdescription.Clear();
            txtdescription.Focus();
        }

        public frmUpdateCourse(string editcoursecode, string editdescription, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editcoursecode = editcoursecode;
            this.editdescription = editdescription;
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
    }
}
