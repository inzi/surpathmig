using System;
using System.Collections.Generic;
using System.Text;
using inzibackend.EntityFrameworkCore;
using inzibackend.Migrations.Seed.Surpath;
using inzibackend.Surpath;
using inzibackend.SurpathSeedHelper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace inzibackend.Migrations.Seed.Host
{
    public static class StringExtensions
    {
        public static string SafeGetString(this MySqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
        public static string SafeGetString(this MySqlDataReader reader, string colName)
        {
            if (!reader.IsDBNull(colName))
                return reader.GetString(colName);
            return string.Empty;
        }
    }
}
