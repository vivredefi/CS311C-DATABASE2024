using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311C_DATABASE2024
{
    public partial class frmUpdateStudent : Form
    {
        private string username, editstudentID, editlastname, editfirstname, editmiddlename, editlevel, editcoursestrand;
        private int errorcount;
        Class1 updatestudent = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private void LoadCoursesAndStrands()
        {
            cmbcoursestrand.DataSource = null; // Clear previous data
            cmbcoursestrand.Items.Clear(); // Clear the items

            if (cmblevel.SelectedItem != null)
            {
                string selectedLevel = cmblevel.SelectedItem.ToString().ToUpper();
                DataTable data;

                if (selectedLevel == "SHS")
                {
                    data = updatestudent.GetData("SELECT description, strandcode FROM tblstrands");
                    cmbcoursestrand.DisplayMember = "description";
                    cmbcoursestrand.ValueMember = "strandcode";
                    cmbcoursestrand.DataSource = data;
                    cmbcoursestrand.Enabled = true;
                }
                else if (selectedLevel == "COLLEGE")
                {
                    data = updatestudent.GetData("SELECT description, coursecode FROM tblcourses");
                    cmbcoursestrand.DisplayMember = "description";
                    cmbcoursestrand.ValueMember = "coursecode";
                    cmbcoursestrand.DataSource = data;
                    cmbcoursestrand.Enabled = true;
                }
                else
                {
                    cmbcoursestrand.Items.Add("N/A");
                    cmbcoursestrand.SelectedIndex = 0;
                    cmbcoursestrand.Enabled = false;
             
                }

                // Set the selected item based on the editcoursestrand after populating
                if (!string.IsNullOrEmpty(editcoursestrand))
                {
                    try
                    {
                        cmbcoursestrand.SelectedValue = editcoursestrand; // Use ValueMember
                    }
                    catch
                    {
                        cmbcoursestrand.SelectedIndex = -1; // Reset if not found
                    }
                }
                else
                {
                    cmbcoursestrand.SelectedIndex = -1; // Reset if nothing matches
                }
            }
        }
        private void frmUpdateStudent_Load(object sender, EventArgs e)
        {
            // Load student details
            txtstudentID.Text = editstudentID;
            txtlastname.Text = editlastname;
            txtfirstname.Text = editfirstname;
            txtmiddlename.Text = editmiddlename;
            cmblevel.SelectedItem = editlevel;

            // Load courses and strands based on the selected level
            LoadCoursesAndStrands();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtlastname.Clear();
            txtfirstname.Clear();
            txtmiddlename.Clear();
            cmblevel.SelectedIndex = -1;
            cmbcoursestrand.Items.Clear();
            cmbcoursestrand.Enabled = false;
            txtlastname.Focus();
        }


        private void validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;

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
                errorProvider1.SetError(cmbcoursestrand, "Select status");
                errorcount++;
            }
        }
        private void cmblevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCoursesAndStrands(); // Call to load corresponding courses/strands
            // Ensure that cmbcoursestrand is enabled if a valid level is selected
            cmbcoursestrand.Enabled = cmblevel.SelectedIndex >= 0;
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this student info?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        // Fetch the selected course/strand code
                        string courseOrStrandCode = cmbcoursestrand.SelectedValue?.ToString();

                        // Debugging output for verification
                        Console.WriteLine($"Selected Course/Strand Code: {courseOrStrandCode}");

                        // Update the student information
                        updatestudent.executeSQL($"UPDATE tblstudents SET lastname = '{txtlastname.Text}', firstname = '{txtfirstname.Text}', middlename = '{txtmiddlename.Text}', level = '{cmblevel.Text.ToUpper()}', course_strand = '{courseOrStrandCode}' WHERE studentID = '{txtstudentID.Text}'");

                        // Check if the update affected rows
                        if (updatestudent.rowAffected > 0)
                        {
                            // Log the update
                            updatestudent.executeSQL($"INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortTimeString()}', 'Update', 'Students Management', '{txtstudentID.Text}', '{username}')");
                            MessageBox.Show("Student Information Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made to the student information.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        public frmUpdateStudent(string editstudentID, string editlastname, string editfirstname, string editmiddlename, string editlevel, string editcoursestrand, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editstudentID = editstudentID;
            this.editlastname = editlastname;
            this.editfirstname = editfirstname;
            this.editmiddlename = editmiddlename;
            this.editlevel = editlevel;
            this.editcoursestrand = editcoursestrand;

            cmblevel.SelectedIndexChanged += new EventHandler(cmblevel_SelectedIndexChanged); // Handle the selection change
        }

    }
}