using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class FormMenu : Form
    {
        FormLog fl;
        FormRules fr;
        Form1 fm1;
        public FormMenu()
        {
            InitializeComponent();
        }

        private void btn_newGame_Click(object sender, EventArgs e)
        {
            fl = new FormLog();
            
            this.Hide();
			fl.ShowDialog();
            this.Show();
		}

        private void btn_Rules_Click(object sender, EventArgs e)
        {
            fr = new FormRules();
            fr.ShowDialog();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
