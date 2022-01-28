﻿using Microsoft.Data.Sqlite;
using PicturePerfect.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public static class Database
    {
        /// <summary>
        /// This class establishes the connection to the sqlite database.
        /// </summary>
        private class SQLiteConnector
        {
            /// <summary>
            /// Indicates weather this instance of the connector has an open open connection to the sqlite file.
            /// </summary>
            private static bool IsConnected = false;
            /// <summary>
            /// Get the connection to the sqlite database.
            /// </summary>
            public SqliteConnection Connection { get; private set; } = new();

            private readonly SqliteConnectionStringBuilder connstringBuilder = new SqliteConnectionStringBuilder();

            /// <summary>
            /// Creates a new instance of the SQLiteConnector.
            /// Opens the connection to the database and sets the Connection property.
            /// </summary>
            public SQLiteConnector()
            {
                if (IsConnected == false)
                {
                    // file exists --> connect to Sqlite
                    connstringBuilder.DataSource = ThisApplication.ProjectFile.DatabasePath;
                    string connstring = connstringBuilder.ToString();

                    Connection = new SqliteConnection(connstring);

                    Connection.Open();
                    IsConnected = true;
                }
                else
                {
                    MessageBox.Show("Internal error while trying to connect to the database. Connection already open.", null, MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
                }
            }

            /// <summary>
            /// Method to close the SQLite connection if currently open.
            /// </summary>
            public void CloseConnection()
            {
                // check if connection is closed
                if (IsConnected == true)
                {
                    // connection is closed
                    Connection.Close();
                    Connection.Dispose();
                    IsConnected = false;
                }
            }
        }

        /// <summary>
        /// Method to create a new database in the selected location.
        /// </summary>
        public static void NewDatabase()
        {
            SQLiteConnector connector = new();

            string[] queries = {"CREATE TABLE images (id INTEGER PRIMARY KEY, name TEXT, subfolder TEXT, file_type TEXT, date_taken TEXT, size REAL, camera TEXT, iso REAL, fstop REAL, exposure_time REAL, exposure_bias REAL, focal_length REAL, notes TEXT)",
                                "CREATE TABLE categories (id INTEGER PRIMARY KEY, name TEXT, notes TEXT)",
                                "CREATE TABLE subcategories (id INTEGER PRIMARY KEY, name TEXT, notes TEXT)",
                                "CREATE TABLE locations (id INTEGER PRIMARY KEY, name TEXT, geo_tag TEXT, notes TEXT)",
                                "CREATE TABLE categories_subcategories (id INTEGER PRIMARY KEY, category_id INTEGER, subcategory_id INTEGER)",
                                "CREATE TABLE images_subcategories (id INTEGER PRIMARY KEY, image_id INTEGER, subcategory_id INTEGER)",
                                "CREATE TABLE images_locations (id INTEGER PRIMARY KEY, image_id INTEGER, location_id INTEGER)"};

            // run queries against database
            foreach (string query in queries)
            {
                var command = new SqliteCommand(query, connector.Connection);
                command.ExecuteNonQuery();
            }

            connector.CloseConnection();
        }
    }
}