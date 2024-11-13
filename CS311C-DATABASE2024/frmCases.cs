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
    public partial class frmCases : Form
    {
        Class1 cases = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private string username;
        public frmCases(string username)
        {
            InitializeComponent();
            this.username = username;

        }
        private int row;
        private DataTable GetStudentData(string studentNumber)
        {
            string query = "SELECT lastname, firstname, middlename, level, course_strand FROM tblstudents WHERE studentID = '" + studentNumber + "'";
            return cases.GetData(query);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Check if the clicked cell is a valid data cell (not the header)
                if (e.RowIndex >= 0)
                {
                    row = e.RowIndex; // Store the row index of the clicked cell
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while selecting the student: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmCases_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Optional: Disable the row headers to save space
            dataGridView1.RowHeadersVisible = false;

            // Optional: Disable adding new rows if not needed
            dataGridView1.AllowUserToAddRows = false;

            // Optional: Allow full row selection
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Optional: Disable user resizing of columns
            dataGridView1.AllowUserToResizeColumns = false;

            // Optional: Set default style to make it more readable
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.DefaultCellStyle.Padding = new Padding(5);

            // Optional: Set the height of the rows
            dataGridView1.RowTemplate.Height = 30;
        }
        private void ClearStudentInformation()
        {
            txtfirstname.Clear();
            txtlastname.Clear();
            txtmiddlename.Clear();
            txtlevel.Clear();
            txtstrandcourse.Clear();
            dataGridView1.DataSource = null;
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            string studentNumber = txtsearch.Text.Trim();

            if (!string.IsNullOrEmpty(studentNumber))
            {
                // Clear the DataGridView before loading data for the searched student
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();

                DataTable studentData = GetStudentData(studentNumber);

                if (studentData.Rows.Count > 0)
                {
                    txtlastname.Text = studentData.Rows[0]["lastname"].ToString();
                    txtfirstname.Text = studentData.Rows[0]["firstname"].ToString();
                    txtmiddlename.Text = studentData.Rows[0]["middlename"].ToString();
                    txtlevel.Text = studentData.Rows[0]["level"].ToString();
                    txtstrandcourse.Text = studentData.Rows[0]["course_strand"].ToString();

                    // Now load the cases for the searched student with updated table structure
                    string query = "SELECT caseID, violationID, status, violation_count, action, school_year, concern_level, disciplinary_action, createdby, datecreated FROM tblcases WHERE studentID = '" + studentNumber + "' ORDER BY caseID DESC";

                    DataTable caseData = cases.GetData(query);

                    // Now add description and vtype from tblviolations based on vcode
                    DataTable resultData = new DataTable();
                    resultData.Columns.Add("caseID");
                    resultData.Columns.Add("violationID");
                    resultData.Columns.Add("description");
                    resultData.Columns.Add("violationtype");
                    resultData.Columns.Add("violation_count");
                    resultData.Columns.Add("status");
                    resultData.Columns.Add("action");
                    resultData.Columns.Add("school_year");
                    resultData.Columns.Add("concern_level");
                    resultData.Columns.Add("disciplinary_action");
                    resultData.Columns.Add("date");
                    foreach (DataRow row in caseData.Rows)
                    {
                        string vcode = row["violationID"].ToString();
                        string violationQuery = "SELECT description, violationtype FROM tblviolations WHERE violationcode = '" + vcode + "'";
                        DataTable violationData = cases.GetData(violationQuery);

                        string description = violationData.Rows.Count > 0 ? violationData.Rows[0]["description"].ToString() : string.Empty;
                        string vtype = violationData.Rows.Count > 0 ? violationData.Rows[0]["violationtype"].ToString() : string.Empty;

                        resultData.Rows.Add(row["caseID"], vcode, description, vtype, row["violation_count"], row["status"], row["action"], row["school_year"], row["concern_level"], row["disciplinary_action"], row["datecreated"]);
                    }

                    dataGridView1.DataSource = resultData;
                }
                else
                {
                    MessageBox.Show("Student not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearStudentInformation();
                }
            }
            else
            {
                MessageBox.Show("Please enter a student number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            ClearStudentInformation();
        }
        private void RefreshCaseData(string studentID)
        {
            string query = "SELECT caseID, violationID, status, violation_count, action, school_year, concern_level, disciplinary_action, createdby, datecreated FROM tblcases WHERE studentID = '" + studentID + "' ORDER BY caseID DESC";
            DataTable caseData = cases.GetData(query);

            DataTable resultData = new DataTable();
            resultData.Columns.Add("caseID");
            resultData.Columns.Add("violationID");
            resultData.Columns.Add("description");
            resultData.Columns.Add("violationtype");
            resultData.Columns.Add("violation_count");
            resultData.Columns.Add("status");
            resultData.Columns.Add("action");
            resultData.Columns.Add("school_year");
            resultData.Columns.Add("concern_level");
            resultData.Columns.Add("disciplinary_action");
            resultData.Columns.Add("date");

            foreach (DataRow row in caseData.Rows)
            {
                string vcode = row["violationID"].ToString();
                string violationQuery = "SELECT description, violationtype FROM tblviolations WHERE violationcode = '" + vcode + "'";
                DataTable violationData = cases.GetData(violationQuery);

                string description = violationData.Rows.Count > 0 ? violationData.Rows[0]["description"].ToString() : string.Empty;
                string vtype = violationData.Rows.Count > 0 ? violationData.Rows[0]["violationtype"].ToString() : string.Empty;

                resultData.Rows.Add(row["caseID"], vcode, description, vtype, row["violation_count"], row["status"], row["action"], row["school_year"], row["concern_level"], row["disciplinary_action"], row["datecreated"]);
            }

            // Rebind the DataGridView with the updated data
            dataGridView1.DataSource = resultData;
        }


        private void btnadd_Click(object sender, EventArgs e)
        {
            // Ensure that a student is searched and student information is populated
            if (!string.IsNullOrEmpty(txtsearch.Text) &&
                !string.IsNullOrEmpty(txtlastname.Text) &&
                !string.IsNullOrEmpty(txtfirstname.Text))
            {
                string studentID = txtsearch.Text.Trim();
                string lastname = txtlastname.Text.Trim();
                string firstname = txtfirstname.Text.Trim();
                string middlename = txtmiddlename.Text.Trim();
                string level = txtlevel.Text.Trim();
                string strandcourse = txtstrandcourse.Text.Trim();

                // Pass DataGridView reference to frmNewCase
                frmNewCase newCaseForm = new frmNewCase(username, studentID, lastname, firstname, middlename, level, strandcourse, dataGridView1);

                // Refresh cases when the form is closed
                newCaseForm.FormClosed += (s, args) => RefreshCaseData(studentID);
                newCaseForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please search for a student and ensure the student information is populated before adding a case.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnupdate_Click(object sender, EventArgs e)
        {
            // Ensure that a student is searched first
            if (!string.IsNullOrEmpty(txtsearch.Text))
            {
                // Ensure that a student is searched first
                if (!string.IsNullOrEmpty(txtsearch.Text))
                {
                    // Check if the student information fields are populated
                    if (!string.IsNullOrEmpty(txtlastname.Text) && !string.IsNullOrEmpty(txtfirstname.Text))
                    {
                        // Check if there are any rows in the DataGridView
                        if (dataGridView1.CurrentRow != null) // Check if a case is selected
                        {
                            string caseID = dataGridView1.CurrentRow.Cells["caseID"].Value.ToString();
                            string studentID = txtsearch.Text.Trim();
                            string lastname = txtlastname.Text.Trim();
                            string firstname = txtfirstname.Text.Trim();
                            string middlename = txtmiddlename.Text.Trim();
                            string level = txtlevel.Text.Trim();
                            string strandcourse = txtstrandcourse.Text.Trim();
                            string vcode = dataGridView1.CurrentRow.Cells["violationID"].Value.ToString();
                            string description = dataGridView1.CurrentRow.Cells["description"].Value.ToString();
                            string vcount = dataGridView1.CurrentRow.Cells["violation_count"].Value.ToString();
                            string status = dataGridView1.CurrentRow.Cells["status"].Value.ToString();
                            string action = dataGridView1.CurrentRow.Cells["action"].Value.ToString();
                            string schoolyear = dataGridView1.CurrentRow.Cells["school_year"].Value.ToString();
                            string concernlevel = dataGridView1.CurrentRow.Cells["concern_level"].Value.ToString();
                            string disciplinaryaction = dataGridView1.CurrentRow.Cells["disciplinary_action"].Value.ToString();

                            // Pass DataGridView reference to frmUpdateCase
                            frmUpdateCase updateCaseForm = new frmUpdateCase(username, caseID, studentID, lastname, firstname, middlename, level, strandcourse, vcode, description, vcount, status, action, schoolyear, concernlevel, disciplinaryaction);

                            // Add this line to refresh the cases when the form is closed
                            updateCaseForm.FormClosed += (s, args) => RefreshCaseData(studentID);
                            updateCaseForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("No cases available for this student.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please ensure the student information is populated before updating a case.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please search for a student before updating a case.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}