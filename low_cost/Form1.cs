using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace low_cost
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadComboBox.Set(ref comboBox1);
            LoadComboBox.Set(ref comboBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string origin = comboBox1.GetItemText(this.comboBox1.SelectedItem);
            string destination = comboBox2.GetItemText(this.comboBox2.SelectedItem);
            string date = dateTimePicker1.Value.ToShortDateString().ToString();
            int passengers = Int32.Parse(textBox1.Text);
        }
    }
}
