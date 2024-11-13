using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311C_DATABASE2024
{
    public partial class frmNewStudent : Form
    {
        private string username;
        private int errorcount;
        Class1 newstudent = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        public frmNewStudent(string username)
        {
            InitializeComponent();
            this.username = username;
            cmblevel.SelectedIndexChanged += new EventHandler(cmblevel_SelectedIndexChanged);
        }
        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtstudentID.Text))
            {
                errorProvider1.SetError(txtstudentID, "Student ID is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtlastname.Text))
            {
                errorProvider1.SetError(txtlastname, "Last Name is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                errorProvider1.SetError(txtfirstname, "First Name is empty");
                errorcount++;
            }
            if (cmblevel.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmblevel, "Select level");
                errorcount++;
            }
            if (cmbcoursestrand.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbcoursestrand, "Select course/strand");
                errorcount++;
            }
            try
            {
                DataTable dt = newstudent.GetData("SELECT * FROM tblstudents WHERE studentID = '" + txtstudentID.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtstudentID, "StudentID is already in use");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating exisiting student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        string courseOrStrandCode = "";

                        // Determine the course/strand code based on the selected level and course/strand description
                        if (cmblevel.Text == "SHS")
                        {
                            string selectedStrand = cmbcoursestrand.SelectedItem.ToString();
                            // Fetch the code for the selected strand description
                            DataTable dt = newstudent.GetData($"SELECT strandcode FROM tblstrands WHERE description = '{selectedStrand}'");
                            if (dt.Rows.Count > 0)
                            {
                                courseOrStrandCode = dt.Rows[0]["strandcode"].ToString(); // Get the strand code
                            }
                        }
                        else if (cmblevel.Text == "COLLEGE")
                        {
                            string selectedCourse = cmbcoursestrand.SelectedItem.ToString();
                            // Fetch the code for the selected course description
                            DataTable dt = newstudent.GetData($"SELECT coursecode FROM tblcourses WHERE description = '{selectedCourse}'");
                            if (dt.Rows.Count > 0)
                            {
                                courseOrStrandCode = dt.Rows[0]["coursecode"].ToString();
                            }
                        }
                        else if (cmblevel.Text == "ELEMENTARY" || cmblevel.Text == "JHS")
                        {
                            courseOrStrandCode = "N/A";
                        }

                        // Debugging: Check the retrieved course/strand code
                        Console.WriteLine("Course/Strand Code: " + courseOrStrandCode);

                        // Execute the insert statement with the course/strand code
                        newstudent.executeSQL("INSERT INTO tblstudents (studentID, lastname, firstname, middlename, level, course_strand, createdby, datecreated) VALUES ('" +
                            txtstudentID.Text + "', '" + txtlastname.Text + "', '" +
                            txtfirstname.Text + "', '" + txtmiddlename.Text + "', '" +
                            cmblevel.Text.ToUpper() + "', '" + courseOrStrandCode.ToUpper() + "', '" + username + "', '" + DateTime.Now.ToShortDateString() + "')");

                        if (newstudent.rowAffected > 0)
                        {
                            newstudent.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                                DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'Add', 'Students Management', '" + txtstudentID.Text + "', '" + username + "')");

                            MessageBox.Show("New Student Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtstudentID.Clear();
            txtlastname.Clear();
            txtfirstname.Clear();
            txtmiddlename.Clear();
            
            cmblevel.SelectedIndex = -1;
            cmbcoursestrand.Items.Clear(); 
            cmbcoursestrand.Enabled = false;
            
            txtstudentID.Focus();
        }

        private void cmblevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbcoursestrand.Items.Clear();
            cmbcoursestrand.Enabled = true; // Enable ComboBox by default
            string selectedLevel = cmblevel.SelectedItem?.ToString(); // Safely get the selected level

            DataTable dt;

            switch (selectedLevel)
            {
                case "ELEMENTARY":
                case "JHS":
                    cmbcoursestrand.Enabled = false;
                    cmbcoursestrand.Items.Add("N/A");
                    cmbcoursestrand.SelectedIndex = 0;
                    break;
                case "SHS":
                    dt = newstudent.GetData("SELECT description FROM tblstrands");
                    cmbcoursestrand.Enabled = true;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            cmbcoursestrand.Items.Add(row["description"].ToString());
                        }
                    }
                    break;
                default:
                    dt = newstudent.GetData("SELECT description FROM tblcourses");
                    cmbcoursestrand.Enabled = true;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            cmbcoursestrand.Items.Add(row["description"].ToString());
                        }
                    }
                    break;
            }
        }

        private void frmNewStudent_Load(object sender, EventArgs e)
        {
            cmbcoursestrand.Enabled = false;
        }

    }
}
