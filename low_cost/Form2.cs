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
    public partial class Form2 : Form
    {
        protected RootObject answer;
        protected int noPassengers;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(RootObject answer, int passengers)
        {
            InitializeComponent();
            this.Size = new Size(850, 450);
            this.answer = answer;
            noPassengers = passengers;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(4);
            for (int i = 0; i != answer.results.Count; ++i)
            {
                dataGridView1.Rows[i].Cells[0].Value = answer.results[i].itineraries[0].inbound.flights[0].origin.airport;
                dataGridView1.Rows[i].Cells[1].Value = answer.results[i].itineraries[0].inbound.flights[0].destination.airport;
                dataGridView1.Rows[i].Cells[2].Value = answer.results[i].itineraries[0].inbound.flights[0].departs_at;
                dataGridView1.Rows[i].Cells[3].Value = answer.results[i].itineraries[0].inbound.flights[0].arrives_at;
                dataGridView1.Rows[i].Cells[4].Value = noPassengers.ToString();
                dataGridView1.Rows[i].Cells[5].Value = answer.currency;
                dataGridView1.Rows[i].Cells[6].Value = answer.results[i].fare.total_price;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
