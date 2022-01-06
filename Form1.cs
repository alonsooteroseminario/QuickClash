using System;
using System.Windows.Forms;

namespace QuickClash
{
    public partial class Form1 : Form
    {
        public string textString { set; get; }
        public Form1()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textString = textBox1.Text;
        }
    }
}
