﻿using System;

namespace Serenity.ComponentModel
{
    /// <summary>
    /// Indicates that the target property should use a "HtmlReportContent" editor.
    /// This is generally a CK editor with only functionality compatible with common
    /// reporting tools like SSRS, Telerik, DevExpress etc. enabled.
    /// </summary>
    /// <seealso cref="Serenity.ComponentModel.CustomEditorAttribute" />
    public partial class HtmlReportContentEditorAttribute : CustomEditorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlReportContentEditorAttribute"/> class.
        /// </summary>
        public HtmlReportContentEditorAttribute()
            : base("HtmlReportContent")
        {
        }

        /// <summary>
        /// Gets or sets the cols for underlying textarea.
        /// </summary>
        /// <value>
        /// The cols.
        /// </value>
        public Int32 Cols
        {
            get { return GetOption<Int32>("cols"); }
            set { SetOption("cols", value); }
        }

        /// <summary>
        /// Gets or sets the rows for underlying textarea.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public Int32 Rows
        {
            get { return GetOption<Int32>("rows"); }
            set { SetOption("rows", value); }
        }
    }
}