using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteEx
{
    internal class ReadFromSQLite
    {
public static string baseName = SQLiteEx.Helpers.Storage.PathFor("BBDD.sqlite");

        public static List<(string Name, string Age)> Persons = new();

        public void Execute()
        {
            Read();

            foreach (var person in Persons)
            {
                Console.WriteLine(person.Name);
                Console.WriteLine(person.Age);
            }
        }

        public static void Read()
        {
            using (SQLiteConnection cnx = new SQLiteConnection("Data Source=" + baseName + ";Version=3;"))
            {
                cnx.Open();

                using (SQLiteCommand cmd = cnx.CreateCommand())
                {
                    cmd.CommandText = "SELECT name, age FROM person";
                    cmd.CommandType = CommandType.Text;

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Persons.Add((
                            reader["name"].ToString() ?? "",
                            reader["age"].ToString() ?? ""
                        ));

                        }
                    }


                }
            }
        }
    }
}
