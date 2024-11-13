using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311C_DATABASE2024
{
    public partial class frmNewCourse : Form
    {
        private string username;
        private int errorcount;
        Class1 newcourse = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmNewCourse(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtcoursecode.Text))
            {
                errorProvider1.SetError(txtcoursecode, "Course Code is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Description is empty");
                errorcount++;
            }
            try
            {
                DataTable dt = newcourse.GetData("SELECT * FROM tblcourses WHERE coursecode = '" + txtcoursecode.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtcoursecode, "Course Code is already in use");
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
                DialogResult dr = MessageBox.Show("Are you sure you want to add this course?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newcourse.executeSQL("INSERT INTO tblcourses (coursecode, description, datecreated, createdby) VALUES ('" + txtcoursecode.Text + "', '" + txtdescription.Text + "', '" +
                            DateTime.Now.ToShortDateString() + "', '" + username + "')");
                        if (newcourse.rowAffected > 0)
                        {
                            newcourse.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'Add', 'Courses Management', '" + txtcoursecode.Text + "', '" + username + "')");

                            MessageBox.Show("New Course Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtcoursecode.Clear();
            txtdescription.Clear();
            txtcoursecode.Focus();
        }

    }
}
