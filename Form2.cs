using System;
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

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.oauth;
            textBox2.Text = Properties.Settings.Default.name;
        }
    }
}
