using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SQLiteEx
{
    public class CreateDatabaseInMemorySQLite 
    {

        public static string ConnectionString =
            "Data Source=:memory:;Version=3;New=True;";

        public void Execute()
        {
            CreateTables();
        }

        public static void CreateTables()
        {
            using (SQLiteConnection cnx = new SQLiteConnection(ConnectionString))
            {
                cnx.Open();

                string sqlTablePerson = "CREATE TABLE person (name VARCHAR (20), age INT)";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlTablePerson, cnx))
                {
                    cmd.ExecuteNonQuery();
                }

                string sqlTableTeacher = "CREATE TABLE teacher (name VARCHAR (20))";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlTableTeacher, cnx))
                {
                    cmd.ExecuteNonQuery();
                }

                // Con esto compruebo que se han creado las tablas.
                string sqListTables = "SELECT name FROM sqlite_master WHERE TYPE = 'table'";

                using (SQLiteCommand cmd = new SQLiteCommand(sqListTables, cnx))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nTablas disponibles en la BBDD en memoria:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader["name"]}");
                    }
                }
            }
        }
    }
}
