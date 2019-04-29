﻿using System;

namespace Serenity.ComponentModel
{
    /// <summary>
    /// Indicates that the target property should use a "DateTime" editor.
    /// </summary>
    /// <seealso cref="Serenity.ComponentModel.CustomEditorAttribute" />
    public partial class DateTimeEditorAttribute : CustomEditorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeEditorAttribute"/> class.
        /// </summary>
        public DateTimeEditorAttribute()
            : base("DateTime")
        {
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public DateTime MinValue
        {
            get { return GetOption<DateTime>("minValue"); }
            set { SetOption("minValue", value); }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public DateTime MaxValue
        {
            get { return GetOption<DateTime>("maxValue"); }
            set { SetOption("maxValue", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether SQL server min max dates should be used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if SQL server min max dates should be used; otherwise, <c>false</c>.
        /// </value>        
        public bool SqlMinMax
        {
            get { return GetOption<bool?>("sqlMinMax") ?? true; }
            set { SetOption("sqlMinMax", value); }
        }

        /// <summary>
        /// Gets or sets the start hour between 0 and 23.
        /// </summary>
        /// <value>
        /// The start hour.
        /// </value>
        public Int32 StartHour
        {
            get { return GetOption<Int32>("startHour"); }
            set { SetOption("startHour", value); }
        }

        /// <summary>
        /// Gets or sets the end hour between 0 and 23.
        /// </summary>
        /// <value>
        /// The end hour.
        /// </value>
        public Int32 EndHour
        {
            get { return GetOption<Int32>("endHour"); }
            set { SetOption("endHour", value); }
        }

        /// <summary>
        /// Gets or sets the interval minutes, default is 5 mins.
        /// </summary>
        /// <value>
        /// The interval minutes.
        /// </value>
        public Int32 IntervalMinutes
        {
            get { return GetOption<Int32>("intervalMinutes"); }
            set { SetOption("intervalMinutes", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the editor should use UTC format.
        /// </summary>
        /// <value>
        ///   <c>true</c> if UTC format should be used; otherwise, <c>false</c>.
        /// </value>
        public Boolean UseUtc
        {
            get { return GetOption<Boolean>("useUtc"); }
            set { SetOption("useUtc", value); }
        }
    }
}