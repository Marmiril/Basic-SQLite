using System;
using System.IO;
using System.Data.SQLite;

namespace SQLiteEx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new CRUDSQLite2().Execute();
        }
    }
}