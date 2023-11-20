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
    public partial class FormLog : Form
    {
        Form1 fm1 = new Form1();
        public FormLog()
        {
            InitializeComponent();
            lbl_playerName.Text = "Введите имя 1-го игрока!";
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fm1.label5.Text = textBox1.Text;
            textBox1.Text = "";
            lbl_playerName.Text = "Введите имя 2-го игрока!";
            button1.Text = "ОК";
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fm1.label6.Text = textBox1.Text;
            this.Hide();
            fm1.ShowDialog();
            this.Close();
        }
    }
}
