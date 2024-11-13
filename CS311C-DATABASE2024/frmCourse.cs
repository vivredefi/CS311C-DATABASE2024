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
    public partial class frmCourse : Form
    {
        private string username, coursecode;
        public frmCourse(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 courses = new Class1("127.0.0.1", "cs311c2024", "balita", "althea");
        private void frmCourse_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = courses.GetData("SELECT coursecode, description, datecreated, createdby FROM tblcourses WHERE coursecode <> '" + coursecode + "' ORDER BY coursecode");
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
                MessageBox.Show(ex.Message, "Error on course load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            frmNewCourse newCourseform = new frmNewCourse(username);
            newCourseform.FormClosed += (s, args) => frmCourse_Load(sender, e); // Refresh when the add form closes
            newCourseform.ShowDialog();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmCourse_Load(sender, e);
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this Course?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    courses.executeSQL("DELETE FROM tblcourses WHERE coursecode = '" + selectedUser + "'");
                    if (courses.rowAffected > 0)
                    {
                        courses.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                               "', 'Delete', 'Courses Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Course Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView after deletion
                        frmCourse_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editcoursecode = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editdescription = dataGridView1.Rows[row].Cells[1].Value.ToString();
            frmUpdateCourse updateCourseform = new frmUpdateCourse(editcoursecode, editdescription, username);
            updateCourseform.FormClosed += (s, args) => frmCourse_Load(sender, e); // Refresh when the update form closes
            updateCourseform.ShowDialog();
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

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = courses.GetData("SELECT coursecode, description, datecreated, createdby FROM tblcourses WHERE coursecode <> '" + coursecode + "' AND (coursecode LIKE '%" + txtsearch.Text +
                    "%' OR description LIKE '%" + txtsearch.Text + "%') ORDER BY coursecode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
    }
}
