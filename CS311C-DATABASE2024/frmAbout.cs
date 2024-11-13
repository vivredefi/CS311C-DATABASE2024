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
    public partial class frmAbout : Form
    {
        private string username;
        public frmAbout(string username)
        {
            InitializeComponent();
            this.username = username;
        }
    }
}
