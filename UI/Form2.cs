using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickClash
{
    public partial class Form2 : Form
    {
        // checked list needs a data soource ( a list) let´s create one.
        public List<string> CheckedListSource1 { get; set; }

        public List<string> CheckedListSource2 { get; set; }

        public bool CheckBox_1 { get; set; }

        public bool CheckBox_2 { get; set; }

        public bool CheckBox_3 { get; set; }

        public CheckedListBox.CheckedItemCollection CheckedItems1 { get; set; }

        public CheckedListBox.CheckedItemCollection CheckedItems2 { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox1.DataSource = CheckedListSource1;
        }

        private void CheckedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox2.DataSource = CheckedListSource2;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // Solo vista activa
        {
            CheckBox_1 = checkBox1.Checked;
            if (CheckBox_1)
            {
                checkBox3.Checked = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CheckedItems1 = checkedListBox1.CheckedItems;
            CheckedItems2 = checkedListBox2.CheckedItems;

            CheckBox_1 = checkBox1.Checked;
            CheckBox_3 = checkBox3.Checked;

            if (!CheckBox_1 && !CheckBox_3)
            {
                MessageBox.Show("Porfavor selecciona correctamente una opción", "Dynoscript");
            }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_3 = checkBox3.Checked;
            if (CheckBox_3)
            {
                checkBox1.Checked = false;
            }
        }
    }
}
