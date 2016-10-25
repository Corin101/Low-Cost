using System;
using System.IO;
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
            Airport data = new Airport();
            data.Origin = comboBox1.GetItemText(comboBox1.SelectedItem);
            data.Destination = comboBox2.GetItemText(comboBox2.SelectedItem);
            data.DepartureDate = dateTimePicker1.Value.ToShortDateString().ToString();
            data.Passengers = Int32.Parse(textBox1.Text);
            SendHttpRequest newQuery = new SendHttpRequest();
            newQuery.send(data);
        }                                               
    }
}
