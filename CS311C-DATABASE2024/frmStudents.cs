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
    public partial class frmStudents : Form
    {
        private string username, studentID = "";
        public frmStudents(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 students = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private void frmStudents_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = students.GetData("SELECT studentID, lastname, firstname, middlename, level, course_strand, datecreated, createdby FROM tblstudents WHERE studentID <> '" + studentID + "' ORDER BY studentID");
                dataGridView1.DataSource = dt;
                // Set AutoSizeColumnsMode to AllCells to ensure columns fit content
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on student load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = students.GetData("SELECT studentID, lastname, firstname, middlename, level, course_strand, createdby, datecreated FROM tblstudents WHERE studentID <> '" + studentID + "' AND (studentID LIKE '%" + txtsearch.Text +
                    "%' OR lastname LIKE '%" + txtsearch.Text + "%' OR course_strand LIKE '%" + txtsearch.Text + "%') ORDER BY studentID");
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnadd_Click(object sender, EventArgs e)
        {
            frmNewStudent newStudentform = new frmNewStudent(username);
            newStudentform.FormClosed += (s, args) => frmStudents_Load(sender, e); // Refresh when the add form closes
            newStudentform.ShowDialog();
 
        }

        private void btndelete_Click_1(object sender, EventArgs e)
        { // Ensure a row is selected
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a student to delete.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete this Student Report?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // Get the studentID from the selected row
                string selectedUser = dataGridView1.CurrentRow.Cells["studentID"].Value.ToString();

                try
                {
                    // Perform the deletion
                    students.executeSQL("DELETE FROM tblstudents WHERE studentID = '" + selectedUser + "'");
                    if (students.rowAffected > 0)
                    {
                        // Log the deletion
                        students.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'Delete', 'Students Management', '" + selectedUser + "', '" + username + "')");

                        MessageBox.Show("Student Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView after deletion
                        frmStudents_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    
        private void btnrefresh_Click_1(object sender, EventArgs e)
        {
            frmStudents_Load(sender, e);
        }
        private int row;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cellclick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            // Get the selected row's index
            int selectedRowIndex = dataGridView1.CurrentRow.Index;

            // Ensure the selected row index is valid
            if (selectedRowIndex < 0 || selectedRowIndex >= dataGridView1.Rows.Count)
            {
                MessageBox.Show("Invalid selection. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Extract data from the selected row
            string editstudentID = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();
            string editlastname = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
            string editfirstname = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            string editmiddlename = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
            string editlevel = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
            string editcoursestrand = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString();
            // Create and open the update form
            frmUpdateStudent updatestudentform = new frmUpdateStudent(editstudentID, editlastname, editfirstname, editmiddlename, editlevel, editcoursestrand, username);
            updatestudentform.FormClosed += (s, args) => frmStudents_Load(sender, e); // Refresh when the update form closes
            updatestudentform.ShowDialog();
        }
    }
}
