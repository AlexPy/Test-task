/*
 * THis program contains output data for all console selects in the all tasks.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_1
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLQueries MySQLTask = new SQLQueries();
            MySQLTask.CreateDB();
            MySQLTask.PushData("input_cities.txt", "input_firms.txt", "input_documents.txt");
            
            MySQLTask.SelectData();
        }
    }
}
