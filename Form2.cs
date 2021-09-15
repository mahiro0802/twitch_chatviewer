using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Twitchコメビュ
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://twitchapps.com/tmi/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.oauth = textBox1.Text;
            Properties.Settings.Default.name = textBox2.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
