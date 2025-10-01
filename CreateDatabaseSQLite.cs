using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SQLiteEx
{
    internal class CreateDatabaseSQLite
    {
        public static string Database00 = @"C:\Users\Ángel\Desktop\VISUAL STUDIO\SQLiteEx\BBDD.sqlite";

        public void Execute()
        {
            CreateDataBaseIfNotExists();
            CreateTablesIfNotExists();
        }

        public static void CreateDataBaseIfNotExists()
        {
            if (!File.Exists(Database00))
            {
                SQLiteConnection.CreateFile(Database00);
            }
        }

        public static void CreateTablesIfNotExists()
        {
            using (SQLiteConnection cnx =
                new SQLiteConnection("Data Source=" + Database00 + ";Version=3;"))
            {
                cnx.Open();

                string sqlTablePerson =
                    "CREATE TABLE IF NOT EXISTS person" +
                    " (name VARCHAR (20), age INT)";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlTablePerson, cnx))
                {
                    cmd.ExecuteNonQuery();
                }


                string sqlTableTeacher =
                    "CREATE TABLE IF NOT EXISTS teacher (name VARCHAR (20))";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlTableTeacher, cnx))
                {
                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
