using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_2
{
    public partial class Form1 : Form
    {
        public Form1()   {   InitializeComponent();   }

        private void Form1_Load(object sender, EventArgs e)    {       }

        private void label1_Click(object sender, EventArgs e)  {       }

        private void label3_Click(object sender, EventArgs e)  {       }


        //"SELECT" button handler handler
        private void button1_Click(object sender, EventArgs e)
        {
            SqlQuery query = new SqlQuery();
            string code = "";

            //check for errors
            for (int i = 0; i < textBox3.Text.Length; i++)
            {
                if(Char.IsLetter(textBox3.Text[i]))
                { continue; }
                MessageBox.Show("Error. Please enter correct data.");
                return;
            }
            for (int i = 0; i < textBox2.Text.Length; i++)
            {
                if (Char.IsDigit(textBox2.Text[i]))
                { continue; }
                MessageBox.Show("Error. Please enter correct data.");
                return;
            }

            for (int i = 0; i < textBox4.Text.Length; i++)
            {
                if (Char.IsDigit(textBox4.Text[i]))
                { continue; }
                MessageBox.Show("Error. Please enter correct data.");
                return;
            }
            
            // generate the "code" string to generate sql query next time
            if(textBox3.Text.Length != 0)
            {
                code += "1";
            }
            else { code += "0"; }
            if(textBox2.Text.Length != 0)
            {
                code += "1";
            }
            else { code += "0"; }
            if(textBox4.Text.Length != 0)
            {
                code += "1";
            }
            else { code += "0"; }

            //ask database about data
            List<string> output = new List<string>();
            try
            {
                output = query.SelectData(textBox3.Text + " " + textBox2.Text + " " + textBox4.Text, code);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            //output data to dataGridView
            int j = 0;
            while(output.Count != 0)
            {
                string[] words = output[j].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                dataGridView1.Rows.Add(words[0], words[1], words[2], words[3], words[4]);
                output.Remove(output.First());             
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e) {        }

        private void textBox2_TextChanged(object sender, EventArgs e) {        }

        private void textBox4_TextChanged(object sender, EventArgs e) {        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)  {        }

        //"Clear" button function
        private void button2_Click(object sender, EventArgs e)  {  dataGridView1.Rows.Clear();  }
    }
}
