using Microsoft.Data.Sqlite;
using PicturePerfect.Views;
using System;
using System.Collections.Generic;

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
        /// Enum for the column numbers of the table images.
        /// </summary>
        private enum TableImagesOrdinals
        {
            Id, // = 0
            CustomName,
            Name,
            Subfolder,
            FileType,
            DateTaken,
            Size,
            Camera,
            ISO,
            FStop,
            ExposureTime,
            ExposureBias,
            FocalLength,
            Notes //  12
        }

        /// <summary>
        /// Enum for column numbers of table categories.
        /// </summary>
        private enum TableCategoriesOrdinals
        {
            Id,
            Name,
            Notes
        }

        /// <summary>
        /// Enum for column numbers of table sub-categories.
        /// </summary>
        private enum TableSubCategoriesOrdinals
        {
            Id,
            Name,
            Notes
        }

        /// <summary>
        /// Enum for column numbers of table locations.
        /// </summary>
        private enum TableLocationsOrdinals
        {
            Id,
            Name,
            Notes
        }

        /// <summary>
        /// Method to create a new database in the selected location.
        /// </summary>
        public static void NewDatabase()
        {
            SQLiteConnector connector = new();

            string[] queries = {"CREATE TABLE images (id INTEGER PRIMARY KEY, custom_name TEXT, name TEXT, subfolder TEXT, file_type TEXT, date_taken TEXT, size REAL, camera TEXT, iso INTEGER, fstop REAL, exposure_time INTEGER, exposure_bias REAL, focal_length REAL, notes TEXT)",
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

        /// <summary>
        /// Method to add a image to the SQLite table "images".
        /// </summary>
        public static void AddImage(ImageFile imageFile)
        {
            //  values and parameters
            List<string> paramters = new() { "@custom_name", "@name", "@subfolder", "@file_type", "@date_taken", "@size", "@camera", "@iso", "@fstop", @"exposure_time", @"exposure_bias", @"focal_length", @"notes" };
            object[] values = { imageFile.CustomName, imageFile.Name, imageFile.Subfolder, imageFile.FileType, imageFile.DateTaken.ToString(), imageFile.Size, imageFile.Camera, imageFile.ISO, imageFile.FStop, imageFile.ExposureTime, imageFile.ExposureBias, imageFile.FocalLength, imageFile.Notes };

            // new command
            SQLiteConnector connector = new();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO images ( custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes) " +
                    " VALUES (@custom_name, @name, @subfolder, @file_type, @date_taken, @size, @camera, @iso, @fstop, @exposure_time, @exposure_bias, @focal_length, @notes)",
                Connection = connector.Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            connector.CloseConnection();           
        }

        /// <summary>
        /// Method to add a new entry to the locations table.
        /// </summary>
        /// <param name="location"></param>
        public static void AddLocation(Locations.Location location)
        {
            //  values and parameters
            List<string> paramters = new() { "@name", "@geo_tag", @"notes" };
            object[] values = { location.Name, location.GeoTag, location.Notes };

            // new command
            SQLiteConnector connector = new();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO locations ( name, geo_tag, notes) " +
                    " VALUES (@name, @geo_tag, @notes)",
                Connection = connector.Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            connector.CloseConnection();
        }

        /// <summary>
        /// Method to add a new entry to the categories table.
        /// </summary>
        /// <param name="category"></param>
        public static void AddCategory(Category category)
        {
            //  values and parameters
            List<string> paramters = new() { "@name", @"notes" };
            object[] values = { category.Name, category.Notes };

            // new command
            SQLiteConnector connector = new();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO categories (name, notes) " +
                    " VALUES (@name, @notes)",
                Connection = connector.Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            connector.CloseConnection();
        }

        /// <summary>
        /// Method to add a new entry to the table subcategories.
        /// </summary>
        /// <param name="subCategory"></param>
        public static void AddSubcategory(SubCategory subCategory)
        {
            //  values and parameters
            List<string> paramters = new() { "@name", @"notes" };
            object[] values = { subCategory.Name, subCategory.Notes };

            // new command
            SQLiteConnector connector = new();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO subcategories ( name, notes) " +
                    " VALUES (@name, @notes)",
                Connection = connector.Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            connector.CloseConnection();
        }

        /// <summary>
        /// Method to link a category with a subcategory in the sqlite database.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="subCategory"></param>
        public static void LinkCategoryToSubCategory(Category category, SubCategory subCategory)
        {
            if (subCategory.Id == 0)
            {
                // category was not read from database --> id is missing (equals 0)
                // get the id
                // Connect to the Sqlite database
                SQLiteConnector connectorSubCategory = new();

                SqliteCommand commandSubCategory = new()
                {
                    CommandText = @"SELECT id FROM subcategories WHERE name= @name",
                    Connection = connectorSubCategory.Connection
                };
                commandSubCategory.Parameters.AddWithValue("@name", subCategory.Name);

                // get the id from the reader, returns 0 if id was not found
                int subCategoryId = Convert.ToInt32(commandSubCategory.ExecuteScalar());

                connectorSubCategory.CloseConnection();

                // set the id according to the database
                subCategory.Id = subCategoryId;
            }

            //  values and parameters
            List<string> paramters = new() { "@category_id", "@subcategory_id"};
            object[] values = { category.Id, subCategory.Id };

            // new command
            SQLiteConnector connector = new();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO categories_subcategories (category_id, subcategory_id) " +
                    " VALUES (@category_id, @subcategory_id)",
                Connection = connector.Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            connector.CloseConnection();
        }

        /// <summary>
        /// Method to link an image file to a location.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="location"></param>
        public static void LinkImageToLocation(ImageFile image, Locations.Location location)
        {
            //  values and parameters
            List<string> paramters = new() { "@image_id", "@location_id" };
            object[] values = { image.Id, location.Id };

            // new command
            SQLiteConnector connector = new();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO images_locations (image_id, name, location_id) " +
                    " VALUES (@image_id, @location_id)",
                Connection = connector.Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            connector.CloseConnection();
        }

        /// <summary>
        /// Method to load all images file data from the database.
        /// </summary>
        /// <returns>Returns the list of image files.</returns>
        public static List<ImageFile> LoadAllImageFiles()
        {
            List<ImageFile> list = new();
            string commandText = @"SELECT id, custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes FROM images ORDER BY date_taken ASC";

            // Connect to the Sqlite database
            SQLiteConnector connector = new();
            SqliteCommand command = new(commandText, connector.Connection);

            // execute reader
            SqliteDataReader reader = command.ExecuteReader();

            // step through reader
            while (reader.Read())
            {
                ImageFile imageFile = new()
                {
                    CustomName = reader.GetString((int)TableImagesOrdinals.CustomName),
                    Subfolder = reader.GetString((int)TableImagesOrdinals.Subfolder),
                    FileType = reader.GetString((int)TableImagesOrdinals.FileType),
                    DateTaken = DateTime.Parse(reader.GetString((int)TableImagesOrdinals.DateTaken)),
                    Size = reader.GetDouble((int)TableImagesOrdinals.Size),
                    Camera = reader.GetString((int)TableImagesOrdinals.Camera),
                    ISO = reader.GetInt32((int)TableImagesOrdinals.ISO),
                    FStop = reader.GetDouble((int)TableImagesOrdinals.FStop),
                    ExposureTime = reader.GetInt32((int)TableImagesOrdinals.ExposureTime),
                    ExposureBias = reader.GetDouble((int)TableImagesOrdinals.ExposureBias),
                    FocalLength = reader.GetDouble((int)TableImagesOrdinals.FocalLength),
                    Notes = reader.GetString((int)TableImagesOrdinals.Notes)
                };
                imageFile.SetId(reader.GetInt32((int)TableImagesOrdinals.Id));
                imageFile.SetFileName(reader.GetString((int)TableImagesOrdinals.Name));

                list.Add(imageFile);
            }

            // close connection
            connector.CloseConnection();

            return list;
        }

        /// <summary>
        /// Method to load all image file data for files without category assignment.
        /// </summary>
        /// <returns>Returns the list of image files.</returns>
        public static List<ImageFile> LoadAllImageFilesWithoutCategory()
        {
            List<ImageFile> list = new();

            return list;
        }

        /// <summary>
        /// Method to load all image file data for files with a specific category assignment.
        /// </summary>
        /// /// <param name="Category"></param>
        /// <returns>Returns the list of image files.</returns>
        public static List<ImageFile> LoadImageFilesByCategory(Category category)
        {
            List<ImageFile> list = new();

            return list;
        }

        /// <summary>
        /// Method to load all image file data for files with a specific sub-category assignment.
        /// </summary>
        /// <param name="subCategory"></param>
        /// <returns>Returns the list of image files.</returns>
        public static List<ImageFile> LoadImageFilesBySubCategory(SubCategory subCategory)
        {
            List<ImageFile> list = new();

            return list;
        }

        /// <summary>
        /// Method to return a list of all categories in the database.
        /// </summary>
        /// <returns>Returns a list containing all category objects.</returns>
        public static List<Category> LoadAllCategories()
        {
            List<Category> list = new();

            string commandText = @"SELECT id, name, notes FROM categories ORDER BY name ASC";

            // Connect to the Sqlite database
            SQLiteConnector connector = new();
            SqliteCommand command = new(commandText, connector.Connection);

            // execute reader
            SqliteDataReader reader = command.ExecuteReader();

            // step through reader and add subcategories
            while (reader.Read())
            {
                Category category = new()
                {
                    Id = reader.GetInt32((int)TableCategoriesOrdinals.Id),
                    Name = reader.GetString((int)TableCategoriesOrdinals.Name),
                    Notes = reader.GetString((int)TableCategoriesOrdinals.Notes)
                };          

                list.Add(category);
            }

            // close connection
            connector.CloseConnection();

            // add sub-categories (this has to be done after closing the connection to the current reader
            foreach (Category category in list)
            {
                // get the sub-categories
                category.SubCategories = LoadSubcategories(category);
            }

            return list;
        }

        /// <summary>
        /// Method to get a list of subcategories for a specific category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Returns a list of sub-categories for the given category.</returns>
        public static List<SubCategory> LoadSubcategories(Category category)
        {
            List<int> subCategoryIds = new();
            List<SubCategory> list = new();

            // query subcategory ids
            string commandText = @"SELECT subcategory_id FROM categories_subcategories WHERE category_id=@category_id";

            // Connect to the Sqlite database
            SQLiteConnector connector = new();

            SqliteCommand command = new(commandText, connector.Connection);
            command.Parameters.AddWithValue("@category_id", category.Id);

            // Sqlite data reader
            // this reader will not contain elements it the category has no subcategories
            SqliteDataReader reader = command.ExecuteReader();
            // step through the reader containing the subcategory ids           
            if (reader.HasRows)
            {
                // create the subcategory list
                while (reader.Read())
                {
                    // index 0 is the subcategory id (only 1 column in reader)
                    int id = reader.GetInt32(0);

                    string commandTextSubCategory = @"SELECT id, name, notes FROM subcategories WHERE id=@id";
                    SqliteCommand commandSubCategory = new(commandTextSubCategory, connector.Connection);
                    commandSubCategory.Parameters.AddWithValue("@id", id);

                    // call the reader for the subcategory by using the subcateory id
                    SqliteDataReader readerSubCategory = commandSubCategory.ExecuteReader();

                    // reader for the subcategory command will only contain one item, since id is unique
                    if (readerSubCategory.HasRows)
                    {
                        // it will be okay to just call reader.Read() without the while because only the first row is filled
                        readerSubCategory.Read();                      
                        SubCategory subCategory = new()
                        {
                            Id = readerSubCategory.GetInt32((int)TableSubCategoriesOrdinals.Id),
                            Name = readerSubCategory.GetString((int)TableSubCategoriesOrdinals.Name),
                            Notes = readerSubCategory.GetString((int)TableSubCategoriesOrdinals.Notes)
                        };
                        list.Add(subCategory);                       
                    }                   
                }
            }
            

            // close connection
            connector.CloseConnection();

            return list;
        }

        /// <summary>
        /// Method to return all locations stored in the database.
        /// </summary>
        /// <returns>REturns the locations object.</returns>
        public static List<Locations.Location> LoadAllLocations()
        {
            List<Locations.Location> locations = new();

            string commandText = @"SELECT id, name, notes FROM locations ORDER BY name ASC";

            // Connect to the Sqlite database
            SQLiteConnector connector = new();
            SqliteCommand command = new(commandText, connector.Connection);

            // execute reader
            SqliteDataReader reader = command.ExecuteReader();

            // step through reader
            while (reader.Read())
            {
                Locations.Location location = new()
                {
                    Name = reader.GetString((int)TableLocationsOrdinals.Name),
                    Notes = reader.GetString((int)TableLocationsOrdinals.Notes)
                };
                location.SetId(reader.GetInt32((int)TableLocationsOrdinals.Id));

                locations.Add(location);
            }

            // close connection
            connector.CloseConnection();

            return locations;
        }
    }
}
