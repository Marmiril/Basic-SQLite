using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteEx.Helpers
{
    internal static class Storage
    {
        private static readonly string Root =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Databases");

        public static string PathFor(string fileName)
        {
            EnsureRoot();
            return Path.Combine(Root, fileName);
        }

        private static void EnsureRoot()
        {
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
        }

    }
}
