using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickClash
{
    public partial class Form2 : Form
    {
        // checked list needs a data soource ( a list) let´s create one.
        public List<string> checkedListSource1 { get; set; }

        public List<string> checkedListSource2 { get; set; }

        public bool checkBox_1 { get; set; }

        public bool checkBox_2 { get; set; }

        public bool checkBox_3 { get; set; }

        public CheckedListBox.CheckedItemCollection checkedItems1 { get; set; }

        public CheckedListBox.CheckedItemCollection checkedItems2 { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

		private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			checkedListBox1.DataSource = checkedListSource1;
		}

		private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			checkedListBox2.DataSource = checkedListSource2;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) // Solo vista activa
		{
			checkBox_1 = checkBox1.Checked;
			if (checkBox_1)
			{
				checkBox3.Checked = false;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			checkedItems1 = checkedListBox1.CheckedItems;
			checkedItems2 = checkedListBox2.CheckedItems;

			checkBox_1 = checkBox1.Checked;
			checkBox_3 = checkBox3.Checked;

			if (!checkBox_1 && !checkBox_3)
			{
				MessageBox.Show("Porfavor selecciona correctamente una opción", "Dynoscript");
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			checkBox_3 = checkBox3.Checked;
			if (checkBox_3)
			{
				checkBox1.Checked = false;
			}
		}
	}
}
