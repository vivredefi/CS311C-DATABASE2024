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
    public partial class frmMain : Form
    {
        private string username, usertype;
        public frmMain(string username, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;
        }
        private void logoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
            frmLogin loginform = new frmLogin();
            loginform.Show();
        }
        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts accountsform = new frmAccounts(username);
            accountsform.MdiParent = this;  
            accountsform.Show();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudents studentsform = new frmStudents(username);
            studentsform.MdiParent = this;
            studentsform.Show();
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCourse coursefrm = new frmCourse(username);
            coursefrm.MdiParent = this;
            coursefrm.Show();
        }

        private void strandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStrand strandfrm = new frmStrand(username);
            strandfrm.MdiParent = this;
            strandfrm.Show();
        }

        private void violationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViolations violationsfrm = new frmViolations(username);
            violationsfrm.MdiParent = this;
            violationsfrm.Show();
        }

        private void casesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCases casesfrm = new frmCases(username);
            casesfrm.MdiParent = this;
            casesfrm.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout aboutfrm = new frmAbout(username);
            aboutfrm.MdiParent = this;
            aboutfrm.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Username: " + username;
            toolStripStatusLabel2.Text = "Usertype: " + usertype;
            if (usertype == "ADMINISTRATOR")
            {
                accountsToolStripMenuItem.Visible = true;
                studentsToolStripMenuItem.Visible = true;
                violationsToolStripMenuItem.Visible = true;
                coursesToolStripMenuItem.Visible = true;
                strandsToolStripMenuItem.Visible = true;
            }
            else if (usertype == "BRANCH ADMINISTRATOR")
            {
                accountsToolStripMenuItem.Visible = false;
                studentsToolStripMenuItem.Visible = true;
                coursesToolStripMenuItem.Visible = true;
                strandsToolStripMenuItem.Visible = true;
                violationsToolStripMenuItem.Visible = true;
            }
            else
            {
                accountsToolStripMenuItem.Visible = false;
                studentsToolStripMenuItem.Visible = false;
                coursesToolStripMenuItem.Visible = false;
                strandsToolStripMenuItem.Visible = false;
                violationsToolStripMenuItem.Visible = true;
            }
        }

    }
}
