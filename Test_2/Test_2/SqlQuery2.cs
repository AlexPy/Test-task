using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Sql;

namespace Test_2
{
    class SqlQuery2
    {
        private string ConnectString = "Data Source=DB.sqlite;Version=3; foreign_keys=true";
        public SortedList<string, int[]> SelectData(string query)
        {          
            SQLiteConnection m_dbConnection = new SQLiteConnection(ConnectString);
            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            SortedList<String, int[]> sl = new SortedList<String, int[]>();          

            /*
             * As the input we have strings formatted like "string year, string month, int sum"
             * So we'd parse this data and push it into sorted list named sl to output correctly. 
             */
            while (reader.Read())
            {
                int year = 0;
                int month = 0;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetValue(i) + "   ");

                    var tmp = reader.GetValue(i);

                    if (tmp.GetType() == typeof(System.String))  //if we have "string - type" variable 
                    {
                        int temp = Convert.ToInt32(reader.GetValue(i));    //convert reader string value to int
                        if (temp / 100 != 0)
                        {
                            //memorise year
                            year = temp;
                            //add new year into list and create array for 12 months this year
                            sl.Add(tmp.ToString(), new int[13]);
                        }
                        else
                        {
                            //memorise month
                            month = temp;
                        }
                    }
                    else
                    {
                        int temp = Convert.ToInt32(reader.GetValue(i));
                        int[] value;
                        //get values (array of months and sums)
                        sl.TryGetValue(year.ToString(), out value);
                        value[month] = temp;
                    }
                }
            }
            //close the connection with DB            
            m_dbConnection.Close();
            return sl;
                 
        }
    }
}
