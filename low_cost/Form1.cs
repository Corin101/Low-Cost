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
using Newtonsoft.Json;

namespace low_cost
{
    public partial class Form1 : Form
    {
        RootObject answer;
        public RootObject Answer() { return answer; }
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(650, 450);
            LoadComboBox.Set(ref comboBox1);
            LoadComboBox.Set(ref comboBox2);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Airport data = new Airport();
            SendHttpRequest newQuery = new SendHttpRequest();

            data.Origin = comboBox1.GetItemText(comboBox1.SelectedItem);
            data.Destination = comboBox2.GetItemText(comboBox2.SelectedItem);
            data.DepartureDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            data.ReturnDate = dateTimePicker2.Value.ToString("yyy-MM-dd");
            data.Passengers = Int32.Parse(textBox1.Text);

            newQuery.send(data);
            newQuery.Parse();
            answer = newQuery.Parse();
            Form2 Form2 = new Form2(answer, data.Passengers);
            Form2.Show();


        }
                                          
    }
}
