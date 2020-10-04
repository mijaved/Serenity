﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Serenity.Data
{
    public abstract partial class Row
    {
        internal int insidePostHandler;
        internal Row originalValues;
        internal Row previousValues;
        internal PropertyChangedEventHandler propertyChanged;
        internal Action<Row> postHandler;
        private Dictionary<String, String> validationErrors;

        internal void RaisePropertyChanged(Field field)
        {
            if (fields.propertyChangedEventArgs == null)
            {
                var args = new PropertyChangedEventArgs[fields.Count + 1];
                for (var i = 0; i < fields.Count; i++)
                {
                    var f = fields[i];
                    args[i] = new PropertyChangedEventArgs(f.propertyName ?? f.Name);
                }
                args[fields.Count] = new PropertyChangedEventArgs("__ROW__");
                fields.propertyChangedEventArgs = args;
            }

            if (ReferenceEquals(null, field))
                propertyChanged(this, fields.propertyChangedEventArgs[fields.Count]);
            else
                propertyChanged(this, fields.propertyChangedEventArgs[field.Index]);
        }

        public Action<Row> PostHandler
        {
            get { return postHandler; }
            set { postHandler = value; }
        }

        public bool IsFieldChanged(Field field)
        {
            return (originalValues != null &&
                    field.IndexCompare(originalValues, this) != 0);
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged += value;
                if (previousValues == null)
                    previousValues = this.CloneRow();
            }
            remove
            {
                propertyChanged -= value;
            }
        }

        public void BeginEdit()
        {
            TrackAssignments = true;

            if (originalValues == null)
                originalValues = this.CloneRow();
        }

        public void CancelEdit()
        {
            if (originalValues != null)
            {
                var original = originalValues;

                originalValues = null;

                for (int i = 0; i < fields.Count; i++)
                    fields[i].CopyNoAssignment(original, this);

                assignedFields = original.assignedFields;

                ClearValidationErrors();
            }
        }

        public void EndEdit()
        {
            if (postHandler != null &&
                originalValues != null)
            {
                if (insidePostHandler > 0)
                    return; // exception daha iyi olabilir mi?

                insidePostHandler++;
                try
                {
                    ClearValidationErrors();
                    postHandler(this);
                    if (HasErrors)
                        throw new Exception("Lütfen satırdaki işaretli alanları düzeltiniz.");
                    originalValues = null;
                }
                finally
                {
                    insidePostHandler--;
                }

                if (PostEnded != null)
                    PostEnded(this, new EventArgs());
            }
            else
            {
                originalValues = null;
                ClearValidationErrors();
            }
        }

        public bool IsEditing
        {
            get { return originalValues != null; }
        }

        public Row OriginalValues
        {
            get { return originalValues ?? this; }
        }

        public Row PreviousValues
        {
            get { return previousValues ?? this; }
        }

        public bool HasPostHandler
        {
            get { return postHandler != null; }
        }

        public EventHandler PostEnded;

        public void AddValidationError(string propertyName, string error)
        {
            if (validationErrors == null)
                validationErrors = new Dictionary<string, string>();

            validationErrors[propertyName ?? String.Empty] = error;
        }

        public void ClearValidationErrors()
        {
            if (validationErrors != null &&
                validationErrors.Count > 0)
            {
                validationErrors.Clear();
            }
        }

        public void RemoveValidationError(string propertyName)
        {
            if (validationErrors != null)
                validationErrors.Remove(propertyName ?? String.Empty);
        }

        public IDictionary<string, string> ValidationErrors
        {
            get { return validationErrors; }
        }

        public bool HasErrors
        {
            get
            {
                return
                    validationErrors != null &&
                    validationErrors.Count > 0;
            }
        }

#if NET45
        string IDataErrorInfo.Error
        {
            get
            {
                string error;
                if (validationErrors != null &&
                    validationErrors.TryGetValue(String.Empty, out error))
                    return error;
                return String.Empty;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string error;
                if (validationErrors != null &&
                    validationErrors.TryGetValue(columnName ?? String.Empty, out error))
                    return error;
                return String.Empty;
            }
        }

        public PropertyDescriptorCollection GetPropertyDescriptors()
        {
            return fields.propertyDescriptors;
        }
#endif
    }
}