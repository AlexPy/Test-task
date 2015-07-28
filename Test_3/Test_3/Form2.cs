using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_3
{
    public partial class Form2 : Form
    {
        public Form2() { InitializeComponent(); }
        private void Form2_Load(object sender, EventArgs e) { } 
        private void button2_Click(object sender, EventArgs e) { dataGridView1.Rows.Clear(); }

        //"SELECT" button handler handler
        private void button1_Click_1(object sender, EventArgs e)
        {
            //output data to dataGridView
            string query = "select strftime('%Y', DOC_DATE), strftime('%m', DOC_DATE), sum(SUM) from DOCUMENT inner join FIRM on DOCUMENT.FIRM_ID=FIRM.FIRM_ID group by strftime(\"%m-%Y\", DOC_DATE)";
            SqlQuery2 sqlQuery = new SqlQuery2();
            SortedList<string, int[]> sl = new SortedList<string, int[]>();
            sl = sqlQuery.SelectData(query);

            //output
            List<string> rows = new List<string>();

            foreach (KeyValuePair<string, int[]> pair in sl)
            {
                string tmp = "";
                tmp += pair.Key + " ";
                for (int i = 1; i <= 12; i++)
                {
                    tmp += " " + pair.Value[i];
                }
                rows.Add(tmp);
            }
            foreach (string str in rows)
            {
                string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                dataGridView1.Rows.Add(words);
            }
        }

    }
}
