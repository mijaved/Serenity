﻿using System;

namespace Serenity.Data.Mapping
{
    /// <summary>
    /// Determines table name for the row.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public TableNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }
    }
}