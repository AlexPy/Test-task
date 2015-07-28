using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SQLite;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Test_2
{
    class SqlQuery
    {
        // @TODO - do checks 
        public List<string> SelectData(string inputStr, string code)
        {
            //output list
            List<string> output = new List<string>();
            //get data from DB           
            output = getDatafromDb(inputStr, code);
            return output;
        }  
      

        private List<string> getDatafromDb(string inputStr, string code)
        {
            string[] words = inputStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> output = new List<string>();
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=DB.sqlite;Version=3; foreign_keys=true");
            m_dbConnection.Open();

            //select data filtred by city_name
            string query = generateQuery(words, code);
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            int i = 0;
            while (reader.Read())
            {
                string tmp = "";
                for (int j = 0; j < reader.FieldCount; j++)
                {
                    tmp += reader.GetValue(j) + " ";
                }
                output.Add(tmp);
                i++;
            }
            return output;

        }

        private string generateQuery(string[] words, string code)
        {
            string query = "select CITY_ID, CITY.NAME, FIRM.NAME, POST_CITY_ID, JUR_CITY_ID from CITY inner join FIRM on CITY_ID=POST_CITY_ID";

            switch (code)
            {
                case "000":
                    {
                        return query;
                    }             
                case "001":
                    {
                        return query + " where JUR_CITY_ID=" + words[0];
                    }
                case "010":
                    {
                        return query + " where POST_CITY_ID=" + words[0];
                    } 
                case "011":
                    {
                        return query + " where POST+CITY_ID=" + words[0] + " and JUR_CITY_ID=" + words[1];
                    }
                case "100":
                    {
                        return query + " where CITY.NAME=\"" + words[0] + "\"";
                    }
                case "101":
                    {
                        return query + " where CITY.NAME=\"" + words[0] + "\" and JUR_CITY_ID=" + words[1];
                    }
                case "110":
                    {
                        return query + " where CITY.NAME=\"" + words[0] + "\" and POST_CITY_ID=" + words[1];
                    }
                case "111":
                    {
                        return query + " where CITY.NAME=\"" + words[0] + "\" and POST_CITY_ID=" + words[1] + " and JUR_CITY_ID=" + words[2];
                    }
            }              
            return query;
        }
    }        
}