﻿namespace Serenity.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A constant criteria object, which only contains a value expression
    /// that would be converted to its string representation in SQL,
    /// not a parameterized value.
    /// </summary>
    /// <seealso cref="Serenity.Data.Criteria" />
    public class ConstantCriteria : Criteria
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantCriteria"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ConstantCriteria(int value)
            : base(value.ToInvariant())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantCriteria"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public ConstantCriteria(IEnumerable<int> values)
            : base(String.Join(",", values))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantCriteria"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ConstantCriteria(long value)
            : base(value.ToInvariant())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantCriteria"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public ConstantCriteria(IEnumerable<long> values)
            : base(String.Join(",", values))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantCriteria"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dialect">The dialect.</param>
        public ConstantCriteria(string value, ISqlDialect dialect = null)
            : base(value.ToSql(dialect))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantCriteria"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="dialect">The dialect.</param>
        public ConstantCriteria(IEnumerable<string> values, ISqlDialect dialect = null)
            : base(String.Join(",", values.Select(x => x.ToSql(dialect))))
        {
        }
    }
}