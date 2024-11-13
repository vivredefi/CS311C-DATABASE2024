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
    public partial class frmUpdateCase : Form
    {
        Class1 updatecase = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private string username, caseID, studentID, lastname, firstname, middlename, level, strandcourse, vcode, description, vcount, status, action, schoolyear, concernlevel, disciplinaryaction;
        private int errorCount;
        public frmUpdateCase(string username, string caseID, string studentID, string lastname, string firstname, string middlename, string level, string strandcourse, string vcode
            , string description, string vcount, string status, string action, string schoolyear, string concernlevel, string disciplinaryaction)
        {
            InitializeComponent();
            this.username = username;
            this.caseID = caseID;
            this.studentID = studentID;
            this.lastname = lastname;
            this.firstname = firstname;
            this.middlename = middlename;
            this.level = level;
            this.strandcourse = strandcourse;
            this.vcode = vcode;
            this.description = description;
            this.vcount = vcount;
            this.status = status;
            this.action = action;
            this.schoolyear = schoolyear;
            this.concernlevel = concernlevel;
            this.disciplinaryaction = disciplinaryaction;
            this.Load += frmUpdateCase_Load;
        }
        private void frmUpdateCase_Load(object sender, EventArgs e)
        {
            LoadViolationIDs();

            // Set the status value
            txtcaseid.Text = caseID;
            txtstudentid.Text = studentID;
            txtlastname.Text = lastname;
            txtfirstname.Text = firstname;
            txtmiddlename.Text = middlename;
            txtlevel.Text = level;
            txtstrandcourse.Text = strandcourse;

            cmbviolationid.SelectedItem = vcode;

            txtviolationdescription.Text = description;
            cmbviolationcount.SelectedItem = vcount;
            cmbstatus.SelectedItem = status;

            // Set the action text value
            txtaction.Text = action;

            txtschoolyear.Text = schoolyear;
            cmbconcernlevel.Text = concernlevel;
            txtdiscipline.Text = disciplinaryaction;
        }
        private void LoadViolationIDs()
        {
            // Clear existing items
            cmbviolationid.Items.Clear();

            // Fetch violation IDs from the database
            string query = "SELECT violationcode FROM tblviolations";  // Adjust the query if needed
            DataTable violationData = updatecase.GetData(query);

            // Populate ComboBox with violation IDs
            foreach (DataRow row in violationData.Rows)
            {
                cmbviolationid.Items.Add(row["violationcode"].ToString());
            }

            // Optionally select the first item or leave it blank
            // cmbviolationid.SelectedIndex = -1; // Uncomment if you want to leave it unselected initially
        }

        private void validations()
        {
            errorProvider1.Clear();
            errorCount = 0;
            if (cmbstatus.SelectedIndex == 1)
            {
                if (string.IsNullOrEmpty(txtaction.Text))
                {
                    errorProvider1.SetError(txtaction, "Action is empty");
                    errorCount++;
                }
            }
            if(cmbstatus.SelectedIndex == -1)
            {
                    errorProvider1.SetError(cmbstatus, "Status is empty");
                    errorCount++;
            }
            if (string.IsNullOrEmpty(txtschoolyear.Text))
            {
                errorProvider1.SetError(txtschoolyear, "School year is empty");
                errorCount++;
            }
            if (cmbconcernlevel.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbconcernlevel, "Concern level is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtdiscipline.Text))
            {
                errorProvider1.SetError(txtdiscipline, "Procedure for disciplinary action is empty");
                errorCount++;
            }
        }
        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if cmbstatus is selected and has a valid value
            if (cmbstatus.SelectedIndex == 0 || cmbstatus.SelectedIndex == -1)
            {
                txtaction.Clear();
                label2.Visible = false;
                txtaction.Visible = false;
                // Check if the selected value is "ONGOING" or "RESOLVED"
            }
            else
            {
                txtaction.Clear();
                label2.Visible = true;
                txtaction.Visible = true;
            }
        }
  
        private void btnsave_Click(object sender, EventArgs e)
        {
            {
               validations();    
                
                if (errorCount == 0)  // If there are no validation errors
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to update this case?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            // Update the case in the database using the provided status and action
                            updatecase.executeSQL("UPDATE tblcases SET status = '" + cmbstatus.Text.ToUpper() + "', action = '" + txtaction.Text.ToUpper() + "', school_year = '" + txtschoolyear.Text.ToUpper()
                            + "', concern_level = '" + cmbconcernlevel.Text.ToUpper() + "', disciplinary_action = '" + txtdiscipline.Text.ToUpper() + "' WHERE caseID = '" + caseID + "'");
                            // Check if the update was successful (i.e., rows were affected)
                            if (updatecase.rowAffected > 0)
                            {
                                // Log the update action into the tbllogs table
                                updatecase.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                    "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'Update', 'Case Management', '" + caseID + "', '" + username + "')");

                                MessageBox.Show("Case Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();  // Close the form after successful update
                            }
                            else
                            {
                                MessageBox.Show("No changes were made to the case.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error updating case", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtschoolyear.Clear();
            txtaction.Clear();
            txtdiscipline.Clear();
            cmbstatus.SelectedIndex = -1;
            cmbconcernlevel.SelectedIndex = -1;
        }
    }
}
