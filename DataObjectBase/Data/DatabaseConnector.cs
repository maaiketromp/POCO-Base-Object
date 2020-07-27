﻿// <copyright file="DatabaseConnector.cs" company="Maaike Tromp">
// Copyright (c) Maaike Tromp. All rights reserved.
// </copyright>

namespace DataObjectBaseLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataObjectBaseLibrary.Interfaces;
    using Microsoft.Data.SqlClient;

    /// <inheritdoc/>
    public class DatabaseConnector : IDatabaseConnector, IDisposable
    {
        private readonly string connectionString;
        private SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnector"/> class using the connection string.
        /// (For development and testing purposes only).
        /// </summary>
        /// <param name="connString">Database connectionString.</param>
        public DatabaseConnector(string connString)
        {
            this.connectionString = connString;
            this.Initialize();
        }

        /// <inheritdoc/>
        public SqlDataReader PrepareAndExecuteQuery(
            string commandText,
            CommandType commandType = CommandType.Text,
            SqlParameter[] parameters = null)
        {
            using SqlCommand cmd = new SqlCommand(commandText, this.connection);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd.ExecuteReader();
        }

        /// <inheritdoc/>
        public int PrepareAndExecuteNonQuery(
            string commandText,
            CommandType type = CommandType.Text,
            SqlParameter[] parameters = null)
        {
            using SqlCommand cmd = new SqlCommand(commandText, this.connection);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.connection.State != 0)
            {
                this.connection.Close();
            }
        }

        private void Initialize()
        {
            this.connection = new SqlConnection(this.connectionString);
            this.connection.Open();
        }
    }
}
