using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class FormRules : Form
    {
        public FormRules()
        {
            InitializeComponent();

            using (StreamReader sr = new StreamReader("Rules.txt"))
            {
                string[] lines = sr.ReadToEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                for(int i = 0; i < lines.Length; i++)
                {
                    listBox1.Items.Add(lines[i]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
