﻿
namespace Serenity.Data
{
    /// <summary>
    /// Helper class for replacing database caret references in format [^ConnectionKey] in SQL expressions.
    /// </summary>
    public class DatabaseCaretReferences
    {
        /// <summary>
        /// Replaces caret references like [^ConnectionKey] in the specified expression with actual database names.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Replaced expression.</returns>
        public static string Replace(string expression)
        {
            if (expression == null || expression.IndexOf('^') < 0)
                return expression;

            return BracketLocator.ReplaceBracketContents(expression, '^', contents =>
            {
                var idx = contents.IndexOf('^');
                if (idx < 0)
                    return contents;

                string connectionKey = null;

                if (idx != 0)
                    connectionKey = contents.Substring(0, idx);

                string databaseName;

                if (!connectionKey.IsEmptyOrNull())
                {
                    databaseName = SqlConnections.GetDatabaseName(connectionKey);
                    if (!string.IsNullOrEmpty(databaseName))
                        return databaseName;
                }

                if (idx < contents.Length - 1)
                    return contents.Substring(idx + 1);

                return contents;
            });
        }
    }
}