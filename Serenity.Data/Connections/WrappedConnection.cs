﻿using System.Data;

namespace Serenity.Data
{
    /// <summary>
    /// Wraps a connection to add current transaction and dialect support.
    /// </summary>
    /// <seealso cref="System.Data.IDbConnection" />
    public class WrappedConnection : IDbConnection
    {
        private IDbConnection actualConnection;
        private bool openedOnce;
        private WrappedTransaction currentTransaction;
        private ISqlDialect dialect;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedConnection"/> class.
        /// </summary>
        /// <param name="actualConnection">The actual connection.</param>
        /// <param name="dialect">The dialect.</param>
        public WrappedConnection(IDbConnection actualConnection, ISqlDialect dialect)
        {
            this.actualConnection = actualConnection;
            this.dialect = dialect;
        }

        /// <summary>
        /// Gets a value indicating whether the connection was opened once.
        /// </summary>
        /// <value>
        ///   <c>true</c> if opened once; otherwise, <c>false</c>.
        /// </value>
        public bool OpenedOnce
        {
            get { return openedOnce; }
        }

        /// <summary>
        /// Gets the actual connection instance.
        /// </summary>
        /// <value>
        /// The actual connection.
        /// </value>
        public IDbConnection ActualConnection
        {
            get { return actualConnection; }
        }

        /// <summary>
        /// Gets or sets the SQL dialect.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        public ISqlDialect Dialect
        {
            get { return dialect; }
            set { dialect = value; }
        }

        /// <summary>
        /// Gets the current transaction.
        /// </summary>
        /// <value>
        /// The current transaction.
        /// </value>
        public WrappedTransaction CurrentTransaction
        {
            get { return currentTransaction; }
        }

        /// <summary>
        /// Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel"></see> value.
        /// </summary>
        /// <param name="il">One of the <see cref="T:System.Data.IsolationLevel"></see> values.</param>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            var actualTransaction = actualConnection.BeginTransaction(il);
            currentTransaction = new WrappedTransaction(this, actualTransaction);
            return currentTransaction;
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        public IDbTransaction BeginTransaction()
        {
            var actualTransaction = actualConnection.BeginTransaction();
            currentTransaction = new WrappedTransaction(this, actualTransaction);
            return currentTransaction;
        }

        internal void Release(WrappedTransaction transaction)
        {
            if (this.currentTransaction == transaction)
            {
                this.currentTransaction = null;
            }
        }

        /// <summary>
        /// Changes the current database for an open Connection object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database.</param>
        public void ChangeDatabase(string databaseName)
        {
            actualConnection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        public void Close()
        {
            actualConnection.Close();
        }

        /// <summary>
        /// Gets or sets the string used to open a database.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return actualConnection.ConnectionString;
            }
            set
            {
                actualConnection.ConnectionString = value;
            }
        }

        public int? CommandTimeout { get; set; }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        public int ConnectionTimeout
        {
            get { return actualConnection.ConnectionTimeout; }
        }

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <returns>
        /// A Command object associated with the connection.
        /// </returns>
        /// <exception cref="System.Exception">
        /// Active transaction for connection is in invalid state! " + 
        ///                         "Connection was probably closed unexpectedly!
        /// or
        /// Can't set transaction for command! " +
        ///                         "Connection was probably closed unexpectedly!
        /// </exception>
        public IDbCommand CreateCommand()
        {
            var command = actualConnection.CreateCommand();
            try
            {
                if (CommandTimeout.HasValue)
                    command.CommandTimeout = CommandTimeout.Value;
                else if (SqlSettings.DefaultCommandTimeout.HasValue)
                    command.CommandTimeout = SqlSettings.DefaultCommandTimeout.Value;

                var transaction = this.currentTransaction != null ? this.currentTransaction.ActualTransaction : null;
                if (transaction != null && transaction.Connection == null)
                    throw new System.Exception("Active transaction for connection is in invalid state! " + 
                        "Connection was probably closed unexpectedly!");

                command.Transaction = transaction;

                if (transaction != null && command.Transaction == null)
                    throw new System.Exception("Can't set transaction for command! " +
                        "Connection was probably closed unexpectedly!");
            }
            catch
            {
                command.Dispose();
                throw;
            }

            return command;
        }

        /// <summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        public string Database
        {
            get { return actualConnection.Database; }
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        public void Open()
        {
            actualConnection.Open();
            openedOnce = true;
        }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        public ConnectionState State
        {
            get { return actualConnection.State; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            actualConnection.Dispose();
        }
    }
}