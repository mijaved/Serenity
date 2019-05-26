﻿
namespace Serenity.Data
{
    public static class SqlSettings
    {
        public static bool AutoQuotedIdentifiers { get; set; }

        public static int? DefaultCommandTimeout { get; set; }

        public static ISqlDialect DefaultDialect = new SqlServer2012Dialect();
    }
}