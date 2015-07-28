/*
 * This program contains SQL queries from Task 1 without GRID-output task.
 * Also it has SQL query from Task 2 without GRID-output task. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.IO;
using System.Collections;

namespace Test_1
{
    class SQLQueries
    {
        //variables for cities, firms and documents columns names
        private string[] CitiesColumnNames = { "CITY_ID", "NAME", "REGION_ID", "AOKRUG_ID", "F_NORTH", "TYPE_OF_LOCAL_SID", "STATUS_OF_LOCAL_SID", "HIGH_POSTINDEX", "LOW_POSTINDEX", "BEGIN_DATE", "END_DATE", "F_INPUT" };
        private string[] FirmsColumnNames = { "FIRM_ID", "NAME", "ALTERN_SYS_CODE", "HOLDING_SYS_CODE", "BUSINESS_PROP", "POST_INDEX", "JUR_INDEX", "INN", "OKPONH", "OKPOKL", "OKOGU", "OKATO", "CONT_STATION", "VAGON_STATION", "RAILWAY_CODE", "DATE_CORR", "USER_ID", "PHONE", "EMAIL", "FAX", "DATE_INN_CLOSE", "OWNERSHIP_SID", "SPECIAL_SIGH", "RESERVE", "VIP_SIGH", "POST_ADDRESS", "JUR_ADDRESS", "POST_CITY_ID", "JUR_CITY_ID", "LIC_NUMBER", "BEGIN_DATE", "END_DATE", "F_INPUT" };
        private string[] DocumentsColumnNames = { "DOC_ID", "DOC_DATE", "SUM", "FIRM_ID" };
        private string ConnectString = "Data Source=DB.sqlite;Version=3; foreign_keys=true";

        private SQLiteConnection m_dbConnection = null;
        // create DB and tables - CITY and FIRM
        public void CreateDB()
        {
            //creating the DB
            SQLiteConnection.CreateFile("DB.sqlite");            

            //creating table CITY
            string query = "create table if not exists CITY (CITY_ID number(10) primary key not null, NAME varchar2(20) not null, REGION_ID number(10) not null, AOKRUG_ID number(10), F_NORTH number(3), TYPE_OF_LOCAL_SID number(10), STATUS_OF_LOCAL_SID number(10) not null, HIGH_POSTINDEX number(6), LOW_POSTINDEX number(6), BEGIN_DATE date, END_DATE date, F_INPUT number(1) not null)";
            CreateTable(query);

            //creating table FIRM           
            query = "create table if not exists FIRM (FIRM_ID number(10) primary key not null, NAME varchar2(120), ALTERN_SYS_CODE number(10), HOLDING_SYS_CODE number(10), BUSINESS_PROP varchar2(255), POST_INDEX number(10), JUR_INDEX number(10), INN number(15), OKPONH number(11), OKPOKL number(11), OKOGU number(10), OKATO number(11), CONT_STATION number(10), VAGON_STATION number(10), RAILWAY_CODE varchar(10), DATE_CORR date, USER_ID number(10), PHONE varchar2(16), EMAIL varchar2(30), FAX varchar2(16), DATE_INN_CLOSE date, OWNERSHIP_SID number(10) not null, SPECIAL_SIGH number(10) not null, RESERVE number(3), VIP_SIGH number(3), POST_ADDRESS varchar2(100), JUR_ADDRESS varchar2(100), POST_CITY_ID number(10), JUR_CITY_ID number(10) not null, LIC_NUMBER varchar2(20), BEGIN_DATE date, END_DATE date, F_INPUT number(1) not null, FOREIGN KEY(POST_CITY_ID) REFERENCES CITY(CITY_ID),FOREIGN KEY(JUR_CITY_ID) REFERENCES CITY(CITY_ID))";
            CreateTable(query);

            //creating table DOCUMENT
            query = "create table if not exists DOCUMENT (DOC_ID primary key not null, DOC_DATE date, SUM number(10), FIRM_ID number(10), FOREIGN KEY(FIRM_ID) REFERENCES FIRM(FIRM_ID))";
            CreateTable(query);
        }

        private void CreateTable(string query)
        {
            m_dbConnection = new SQLiteConnection(ConnectString);
            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        public void PushData(string filename_cities, string filename_firms, string filename_documents)
        {

            //push cities data into db           
            string writeToDb = "insert into CITY (CITY_ID, NAME, REGION_ID, AOKRUG_ID, F_NORTH, TYPE_OF_LOCAL_SID, STATUS_OF_LOCAL_SID, HIGH_POSTINDEX, LOW_POSTINDEX, BEGIN_DATE, END_DATE, F_INPUT) VALUES ($CITY_ID, $NAME, $REGION_ID, $AOKRUG_ID, $F_NORTH, $TYPE_OF_LOCAL_SID, $STATUS_OF_LOCAL_SID, $HIGH_POSTINDEX, $LOW_POSTINDEX, $BEGIN_DATE, $END_DATE, $F_INPUT )";
            Push(filename_cities, writeToDb);

            //push firms data
            writeToDb = "insert into FIRM (FIRM_ID, NAME, ALTERN_SYS_CODE, HOLDING_SYS_CODE, BUSINESS_PROP, POST_INDEX, JUR_INDEX, INN, OKPONH, OKPOKL, OKOGU, OKATO, CONT_STATION, VAGON_STATION, RAILWAY_CODE, DATE_CORR, USER_ID, PHONE, EMAIL, FAX, DATE_INN_CLOSE, OWNERSHIP_SID, SPECIAL_SIGH, RESERVE, VIP_SIGH, POST_ADDRESS, JUR_ADDRESS, POST_CITY_ID, JUR_CITY_ID, LIC_NUMBER, BEGIN_DATE, END_DATE, F_INPUT) VALUES ($FIRM_ID, $NAME, $ALTERN_SYS_CODE, $HOLDING_SYS_CODE, $BUSINESS_PROP, $POST_INDEX, $JUR_INDEX, $INN, $OKPONH, $OKPOKL, $OKOGU, $OKATO, $CONT_STATION, $VAGON_STATION, $RAILWAY_CODE, $DATE_CORR, $USER_ID, $PHONE, $EMAIL, $FAX, $DATE_INN_CLOSE, $OWNERSHIP_SID, $SPECIAL_SIGH, $RESERVE, $VIP_SIGH, $POST_ADDRESS, $JUR_ADDRESS, $POST_CITY_ID, $JUR_CITY_ID, $LIC_NUMBER, $BEGIN_DATE, $END_DATE, $F_INPUT )";
            Push(filename_firms, writeToDb);   

            //push documents data            
            writeToDb = "insert into DOCUMENT (DOC_ID, DOC_DATE, SUM, FIRM_ID) VALUES ($DOC_ID, $DOC_DATE, $SUM, $FIRM_ID)";
            Push(filename_documents, writeToDb);
        }

        private void Push(string filename, string query)
        {
            //parse filename e.g. 'input_cities.txt' to 'cities'
            string[] parts = filename.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            string[] part = parts[1].Split(new char[] { '.' });
            string[] tableColumns = { };
            switch(part[0])
            {
                case "cities":
                    {
                        tableColumns = CitiesColumnNames;
                    } break;
                case "firms":
                    {
                        tableColumns = FirmsColumnNames;
                    } break;
                case "documents" :
                    {
                        tableColumns = DocumentsColumnNames;
                    } break;
            }

            //read data from file
            string[] text = File.ReadAllLines(filename);
            //open conenction with DB
            m_dbConnection = new SQLiteConnection(ConnectString);
            m_dbConnection.Open();

            foreach (string str in text)
            {
                //parse str into words by space
                string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                //query for insert                
                SQLiteCommand cmd = new SQLiteCommand(query, m_dbConnection);
                //associate names of tables in query with "word" variable parsed from "str" variable
                for (int i = 0; i < tableColumns.Count(); i++)
                {
                    //parametrising
                    cmd.Parameters.AddWithValue("$" + tableColumns[i], words[i]);
                }
                cmd.ExecuteNonQuery();
            }
            m_dbConnection.Close();
        }


        public void SelectData()
        {
            //select data filtred by city_name
            string query = "select CITY_ID, CITY.NAME, FIRM.NAME, POST_CITY_ID, JUR_CITY_ID from CITY inner join FIRM on CITY_ID=POST_CITY_ID where CITY.NAME=\"CHELYABINSK\"";
            Select(query);


            //select data filtred by post_city_id and jur_city_id
            query = "select * from CITY inner join FIRM on CITY_ID=POST_CITY_ID where POST_CITY_ID=4 and JUR_CITY_ID=4";
            Select(query);

            //select data filtred by city_name
            query = "select * from CITY inner join FIRM on CITY_ID=POST_CITY_ID where POST_CITY_ID=1 and JUR_CITY_ID=3 and CITY.NAME=\"CHELYABINSK\"";
            Select(query);

            //select data from DOCUMENTS
            query = "select strftime('%Y', DOC_DATE), strftime('%m', DOC_DATE), sum(SUM) from DOCUMENT inner join FIRM on DOCUMENT.FIRM_ID=FIRM.FIRM_ID group by strftime(\"%m-%Y\", DOC_DATE)";
            Select(query, true);
                      
        }

        private void Select(string query)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection(ConnectString);
            m_dbConnection.Open();

            //select data filtred by city_name
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetValue(i) + "   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("=========================================================");
        }

        private void Select(string query, bool flag)
        {
            m_dbConnection = new SQLiteConnection(ConnectString);
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

            //output
            Console.WriteLine(" Year/  January   February   March   April   May   June   July   August   September   October   November   December");
            foreach (KeyValuePair<string, int[]> pair in sl)
            {
                Console.Write(pair.Key + "   ");
                for (int i = 1; i <= 12; i++)
                {
                    Console.Write("   " + pair.Value[i]);
                }
                Console.WriteLine();
            }
        }
    //close the class
    }
//close the namespace
}