﻿using System;

namespace Serenity.Data.Mapping
{
    /// <summary>
    /// Adds a OUTER APPLY to the row
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="Serenity.Data.Mapping.ISqlJoin" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OuterApplyAttribute : Attribute, ISqlJoin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OuterApplyAttribute"/> class.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="innerQuery">The inner query.</param>
        public OuterApplyAttribute(string alias, string innerQuery)
        {
            this.Alias = alias;
            this.InnerQuery = innerQuery;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public String Alias { get; private set; }

        /// <summary>
        /// Gets the inner query.
        /// </summary>
        /// <value>
        /// The inner query.
        /// </value>
        public String InnerQuery { get; private set; }

        /// <summary>
        /// Gets the property prefix.
        /// </summary>
        /// <value>
        /// The property prefix.
        /// </value>
        public String PropertyPrefix { get; set; }

        /// <summary>
        /// Gets or sets the title prefix.
        /// </summary>
        /// <value>
        /// The title prefix.
        /// </value>
        public String TitlePrefix { get; set; }

        /// <summary>
        /// Gets or sets the type of the row.
        /// </summary>
        /// <value>
        /// The type of the row.
        /// </value>
        public Type RowType { get; set; }

        string ISqlJoin.OnCriteria => InnerQuery;
        string ISqlJoin.ToTable => null;
    }
}