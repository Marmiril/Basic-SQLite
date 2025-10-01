using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SQLiteEx
{
    internal class CRUDSQLite2
    {
public static string baseName = SQLiteEx.Helpers.Storage.PathFor("BBDD.sqlite");
        public static int optionMenu = -1;

        public void Execute()
        {
            CreateDatabaseIfNotExists();
            CreateTablesIfNotExist();

            while(true)
            {
                optionMenu = ShowMenu();
                if (optionMenu == 0) break; // ESC.
                if (optionMenu == -1) continue; // Entrada inválida y continua.

                switch (optionMenu)
                {
                    case 1: Add();    break;
                    case 2: Show();   break;
                    case 3: Edit();   break;
                    case 4: Delete(); break;
                }
            }

        }

        static int ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("\n1 - Add\n2 - Show\n3 - Edit\n4 - Delete\nESC - Exit");
            Console.WriteLine("Enter an option: ");

            var key = Console.ReadKey(intercept: true);
            Console.Clear();

            return key.Key switch 
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => 1,
                ConsoleKey.D2 or ConsoleKey.NumPad2 => 2,
                ConsoleKey.D3 or ConsoleKey.NumPad3 => 3,
                ConsoleKey.D4 or ConsoleKey.NumPad4 => 4,
                ConsoleKey.Escape                   => 0,
                _                                   => -1
            };
        }

        static void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(baseName))
            {
                SQLiteConnection.CreateFile(baseName);
            }
        }

        static void CreateTablesIfNotExist()
        {
            ExecuteNonQuery(
                @"CREATE TABLE IF NOT EXISTS persona(
                    cod INTEGER PRIMARY KEY AUTOINCREMENT,
                    name VARCHAR (20),
                    age INT
                )",
                cmd => { }
            );                
        }

        private static SQLiteConnection OpenConnection()
        {
            var cnx = new SQLiteConnection("Data Source=" + baseName + ";Version=3;");
            cnx.Open();
            return cnx;

        }

        private static void ExecuteNonQuery(string sql, Action<SQLiteCommand> paramSetter)
        {
            using var cnx = OpenConnection();
            using var cmd = new SQLiteCommand(sql, cnx);

            paramSetter(cmd);

            cmd.ExecuteNonQuery();
        }

        private static void ExecuteReader(string sql, Action<SQLiteDataReader> rowHandler, Action<SQLiteCommand>? paramSetter = null)
        {
            using var cnx = OpenConnection();
            using var cmd = new SQLiteCommand(sql, cnx);

            paramSetter?.Invoke(cmd);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                rowHandler(reader);
            }
        }

        static bool ExistsPerson(int cod)
        {
            var exists = false;

            ExecuteReader(
                "SELECT 1 FROM persona WHERE cod=@cod LIMIT 1",
                _ => { exists = true; },
                cmd => cmd.Parameters.AddWithValue("@cod", cod)
                );
            return exists;
        }

        static string? ReadStringKeyed(string prompt)
        {
            Console.Write(prompt);
            var sb = new StringBuilder();

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Escape) return null; // Aquí cancela.
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return sb.ToString();
                }
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

        static int? ReadIntKeyed(string prompt)
        {
            while (true)
            {
                var s = ReadStringKeyed(prompt);
                if (s is null) return null; // Cancelado con Esc.
                if (int.TryParse(s, out var n)) return n;
                Console.WriteLine("Invalid number. Try again.");
            }
        }

        static void Add()
        {
            Console.Clear();
            Console.WriteLine("ADD");

            var name = ReadStringKeyed("Name: ");
            if (name is null) return;

            var age = ReadIntKeyed("Age:");
            if (age is null) return;

            ExecuteNonQuery(
                "INSERT INTO persona(name, age) VALUES (@name, @age)",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@age", age.Value);
                }
            );          
        }

        static void Show()
        {
            Console.Clear();
            Console.WriteLine("SHOW");

            ExecuteReader(
                "SELECT cod, name, age FROM persona ORDER BY cod",
                r => Console.WriteLine($"Cod: {r["cod"]}, Name: {r["name"]}, Age: {r["age"]}")
            );

            Console.WriteLine("\n Press any key to EXIT...");
            Console.ReadKey(intercept: true);
        }

        static void Edit()
        {
            Console.Clear();
            Console.WriteLine("EDIT");

            var cod = ReadIntKeyed("Cod:");

            if (cod is null) return;

            if (!ExistsPerson(cod.Value))
            {
                Console.WriteLine("COD not found");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey(intercept: true);
                return;
            }

            var name = ReadStringKeyed("New name: ");
            if (name is null) return;

            var age = ReadIntKeyed("New age: ");
            if (age is null) return;

            ExecuteNonQuery(
                "UPDATE persona SET name=@name, age=@age WHERE cod=@cod",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@age", age.Value);
                    cmd.Parameters.AddWithValue("@cod", cod.Value);
                }
            );
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("DELETE");

            var cod = ReadIntKeyed("COD: ");

            if (cod is null) return;

            if (!ExistsPerson(cod.Value))
            {
                Console.WriteLine("Cod not found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey(intercept: true);
                return;
            }

            ExecuteNonQuery(
                "DELETE FROM persona WHERE cod=@cod",
                cmd => cmd.Parameters.AddWithValue("@cod", cod)
            );
        }
    }
}
