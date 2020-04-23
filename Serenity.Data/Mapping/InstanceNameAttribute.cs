﻿using System;

namespace Serenity.Data
{
    /// <summary>
    /// Determines non-plural name for an entity.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class InstanceNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNameAttribute"/> class.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        public InstanceNameAttribute(string instanceName)
        {
            this.InstanceName = instanceName;
        }

        /// <summary>
        /// Gets the name of the instance.
        /// </summary>
        /// <value>
        /// The name of the instance.
        /// </value>
        public string InstanceName { get; private set; }
    }
}