using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SQLiteEx
{
    internal class CRUDSQLite
    {
public static string baseName = SQLiteEx.Helpers.Storage.PathFor("BBDD.sqlite");
        public static int OptionMenu = -1;

        public void Execute()
        {
            CreateDataBaseIfNotExists();
            CreateTablesIfNotExists();

            do
            {
                ShowMenu();

                switch (OptionMenu)
                {
                    case 1: Add(); break;
                    case 2: Show(); break;
                    case 3: Edit(); break;
                    case 4: Delete(); break;
                }

            }
            while (OptionMenu != 5);
        }

        public static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Add");
            Console.WriteLine("2 - Show");
            Console.WriteLine("3 - Edit");
            Console.WriteLine("4 - Delete");
            Console.WriteLine("5 - Exit");

            Console.WriteLine();
            Console.WriteLine("Enter an option: ");

            OptionMenu = int.Parse(Console.ReadLine());

            Console.Clear();
        }

        public static void CreateDataBaseIfNotExists()
        {
            if (!File.Exists(baseName))
            {
                SQLiteConnection.CreateFile(baseName);
            }
        }

        public static void CreateTablesIfNotExists()
        {
            using (SQLiteConnection cnx =
                new SQLiteConnection("Data Source=" + baseName + ";Version=3;"))
            {
                cnx.Open();

                string sql = "CREATE TABLE IF NOT EXISTS persona(" +
                    "cod INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name VARCHAR(20), age INT)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Add()
        {
            Console.Clear();
            Console.WriteLine("Add");
            Console.WriteLine();

            Console.WriteLine("Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Age: ");
            int age = int.Parse(Console.ReadLine());

            using (SQLiteConnection cnx =
                new SQLiteConnection("Data Source=" + baseName + ";Version=3;"))
            {
                cnx.Open();

                string sql = "INSERT INTO persona(name, age) VALUES ('" + name + "'," + age + ")";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Show()
        {
            Console.Clear();
            Console.WriteLine("Show");
            Console.WriteLine();

            using (SQLiteConnection cnx =
                new SQLiteConnection("Data Source=" + baseName + ";Version=3"))
            {
                cnx.Open();

                using (SQLiteCommand cmd = cnx.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM persona";
                    cmd.CommandType = CommandType.Text;

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Cod: " + reader["cod"].ToString());
                            Console.WriteLine("Cod: " + reader["name"].ToString());
                            Console.WriteLine("Cod: " + reader["age"].ToString());
                            Console.WriteLine();
                        }
                    }                      
                }
            }
        }

        public static void Edit()
        {
            Console.Clear();
            Console.WriteLine("Edit");
            Console.WriteLine();

            Console.WriteLine("Cod: ");
            int codPerson = int.Parse(Console.ReadLine());

            if (existPerson(codPerson))
            {
                Console.WriteLine("Name: ");
                string newName = Console.ReadLine();

                Console.WriteLine("Age: ");
                int newAge = int.Parse(Console.ReadLine());

                using (SQLiteConnection cnx =
                    new SQLiteConnection("Data Source=" + baseName + ";Version=3;"))
                {
                    cnx.Open();

                    string sql = "UPDATE persona SET name='" + newName + "'," + "age="
                        + newAge + " WHERE cod=" + codPerson;

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                Console.WriteLine("Person not found.");
            }
        }

        public static void Delete()
        {
            Console.Clear();
            Console.WriteLine("Delete");
            Console.WriteLine();

            Console.WriteLine("Cod: ");
            int codPerson = int.Parse(Console.ReadLine());

            if (existPerson(codPerson))
            {
                using (SQLiteConnection cnx =
                    new SQLiteConnection("Data Source=" + baseName + ";Version=3;"))
                {
                    cnx.Open();

                    string sql = "DELETE from persona WHERE cod=" + codPerson;

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                    {
                        cmd.ExecuteNonQuery();
                    }
                } 
            }
            else
            {
                Console.WriteLine("Person not found.");
            }
            
        }

        public static bool existPerson(int codPerson)
        {
            bool exist = false;

            using (SQLiteConnection cnx =
                new SQLiteConnection("Data Source=" + baseName + ";Version=3;"))
            {
                cnx.Open();
                using (SQLiteCommand cmd = cnx.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 AS TOTAL FROM persona WHERE cod=" + codPerson;

                    cmd.CommandType = CommandType.Text;

                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        exist = reader["total"].ToString() == "1" ? true : false;
                    }
                }
            }
            return exist;
        }
    }
}
