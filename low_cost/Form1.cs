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

        }
    }
}
