using Microsoft.Data.Sqlite;
using PicturePerfect.Views;
using System;
using System.Collections.Generic;

namespace PicturePerfect.Models
{
    public static class Database
    {
        /// <summary>
        /// Get or set the connection to the database.
        /// </summary>
        private static SqliteConnection Connection { get; } = SQLiteConnector.GetConnection();

        /// <summary>
        /// Class to connect to the sqlite database.
        /// </summary>
        private static class SQLiteConnector
        {
            /// <summary>
            /// Method to open the connection against the database.
            /// </summary>
            /// <returns>Returns the SqliteConnection object.</returns>
            public static SqliteConnection GetConnection()
            {
                SqliteConnectionStringBuilder connstringBuilder = new();
                connstringBuilder.DataSource = ThisApplication.ProjectFile.DatabasePath;
                string connstring = connstringBuilder.ToString();

                SqliteConnection connection = new(connstring);
                
                return connection;
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
            Connection.Open();
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
                var command = new SqliteCommand(query, Connection);
                command.ExecuteNonQuery();
            }
            Connection.Close();          
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
            Connection.Open();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO images ( custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes) " +
                    " VALUES (@custom_name, @name, @subfolder, @file_type, @date_taken, @size, @camera, @iso, @fstop, @exposure_time, @exposure_bias, @focal_length, @notes)",
                Connection = Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            Connection.Close();
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
            Connection.Open();
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO locations ( name, geo_tag, notes) " +
                    " VALUES (@name, @geo_tag, @notes)",
                Connection = Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            Connection.Close();     
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

            Connection.Open();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO categories (name, notes) " +
                    " VALUES (@name, @notes)",
                Connection = Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            Connection.Close();       
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

            Connection.Open();
            // new command
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO subcategories ( name, notes) " +
                    " VALUES (@name, @notes)",
                Connection = Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            Connection.Close();         
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
                Connection.Open();
                SqliteCommand commandSubCategory = new()
                {
                    CommandText = @"SELECT id FROM subcategories WHERE name= @name",
                    Connection = Connection
                };
                commandSubCategory.Parameters.AddWithValue("@name", subCategory.Name);

                // get the id from the reader, returns 0 if id was not found
                int subCategoryId = Convert.ToInt32(commandSubCategory.ExecuteScalar());

                Connection.Close();

                // set the id according to the database
                subCategory.Id = subCategoryId;              
            }

            //  values and parameters
            List<string> paramters = new() { "@category_id", "@subcategory_id"};
            object[] values = { category.Id, subCategory.Id };

            // new command
            Connection.Open();
            SqliteCommand command = new()
            {
                CommandText = "INSERT INTO categories_subcategories (category_id, subcategory_id) " +
                    " VALUES (@category_id, @subcategory_id)",
                Connection = Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute command and close connection
            command.ExecuteNonQuery();
            Connection.Close();       
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
            Connection.Open();

            // delete old link
            SqliteCommand commandDelete = new()
            {
                CommandText = "DELETE FROM images_locations WHERE image_id=@image_id",
                Connection = Connection
            };
            commandDelete.Parameters.AddWithValue("@image_id", image.Id);
            commandDelete.ExecuteNonQuery();


            // insert new row linking the image and the location
            SqliteCommand commandNew = new()
            {
                CommandText = "INSERT INTO images_locations (image_id, location_id) " +
                    " VALUES (@image_id, @location_id)",
                Connection = Connection
            };
            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => commandNew.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));
            commandNew.ExecuteNonQuery();

            // close connection
            Connection.Close();
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
            Connection.Open();
            SqliteCommand command = new(commandText, Connection);

            // execute reader
            SqliteDataReader reader = command.ExecuteReader();

            // step through reader
            while (reader.Read())
            {
                int id = reader.GetInt32((int)TableImagesOrdinals.Id);
                string customName = reader.GetString((int)TableImagesOrdinals.CustomName);
                string name = reader.GetString((int)TableImagesOrdinals.Name);
                string subfolderName = reader.GetString((int)TableImagesOrdinals.Subfolder);
                string fileType = reader.GetString((int)TableImagesOrdinals.FileType);
                DateTime dateTaken = DateTime.Parse(reader.GetString((int)TableImagesOrdinals.DateTaken));
                double size = reader.GetDouble((int)TableImagesOrdinals.Size);
                string camera = reader.GetString((int)TableImagesOrdinals.Camera);
                int iso = reader.GetInt32((int)TableImagesOrdinals.ISO);
                double fStop = reader.GetDouble((int)TableImagesOrdinals.FStop);
                int exposureTime = reader.GetInt32((int)TableImagesOrdinals.ExposureTime);
                double exposureBias = reader.GetDouble((int)TableImagesOrdinals.ExposureBias);
                double focalLength = reader.GetDouble((int)TableImagesOrdinals.FocalLength);
                string notes = reader.GetString((int)TableImagesOrdinals.Notes);

                Locations.Location location = GetLocation(id);

                ImageFile imageFile = ImageFile.NewFromDatabase(id, name, customName, subfolderName, fileType, dateTaken, size, camera, fStop, iso, exposureTime, exposureBias, focalLength, notes, location);

                list.Add(imageFile);
            }

            
            // add locations           
            //list.ForEach(item => item.Location = GetLocation(item.Id));


            // close connection
            Connection.Close();

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
        /// Method to get the location for a specific image file.
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns>Returns the location object.</returns>
        private static Locations.Location GetLocation(int imageId)
        {
            Locations.Location location = new();

            // query subcategory ids
            string commandText = @"SELECT location_id FROM images_locations WHERE image_id=@image_id";

            // Connect to the Sqlite database
            SqliteCommand command = new(commandText, Connection);
            command.Parameters.AddWithValue("@image_id", imageId);

            // Sqlite data reader
            // this reader will not contain elements if the image has no location assigned
            SqliteDataReader reader = command.ExecuteReader();
            // step through the reader containing the subcategory ids           
            if (reader.HasRows)
            {
                // reader will only have one item since a image can only have one location
                reader.Read();
                int id = reader.GetInt32(0); // index 0 is the location id

                string commandTextLocation = @"SELECT id, name, geo_tag, notes FROM locations WHERE id=@id";
                SqliteCommand commandLocation = new(commandTextLocation, Connection);
                commandLocation.Parameters.AddWithValue("@id", id);
                // call the reader for the location by using the location id
                SqliteDataReader readerLocation = commandLocation.ExecuteReader();

                // reader for the location command will only contain one item, since id is unique
                if (readerLocation.HasRows)
                {
                    readerLocation.Read();
                    // set the properties of the location object
                    location.Id = readerLocation.GetInt32(0);
                    location.Name = readerLocation.GetString(1);
                    location.GeoTag = readerLocation.GetString(2);
                    location.Notes = readerLocation.GetString(3);
                }
            }

            return location;
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
            Connection.Open();
            SqliteCommand command = new(commandText, Connection);

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
            Connection.Close();

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
            List<SubCategory> list = new();

            // query subcategory ids
            string commandText = @"SELECT subcategory_id FROM categories_subcategories WHERE category_id=@category_id";

            // Connect to the Sqlite database
            Connection.Open();

            SqliteCommand command = new(commandText, Connection);
            command.Parameters.AddWithValue("@category_id", category.Id);

            // Sqlite data reader
            // this reader will not contain elements if the category has no subcategories
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
                    SqliteCommand commandSubCategory = new(commandTextSubCategory, Connection);
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
            Connection.Close();

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
            Connection.Open();
            SqliteCommand command = new(commandText, Connection);

            // execute reader
            SqliteDataReader reader = command.ExecuteReader();

            // step through reader
            while (reader.Read())
            {
                Locations.Location location = new()
                {
                    Id = reader.GetInt32((int)TableLocationsOrdinals.Id),
                    Name = reader.GetString((int)TableLocationsOrdinals.Name),
                    Notes = reader.GetString((int)TableLocationsOrdinals.Notes)
                };

                locations.Add(location);
            }

            // close connection
            Connection.Close();

            return locations;
        }

        /// <summary>
        /// Method to set the custim name of a given image file in the sqlite database.
        /// </summary>
        /// <param name="imageFile"></param>
        public static void SetCustomName(ImageFile imageFile)
        {
            // prepare values
            List<string> paramters = new() { "@id", "@custom_name"};
            object[] values = { imageFile.Id, imageFile.CustomName };

            // connect to sqlite database
            Connection.Open();

            // set command and its properties
            SqliteCommand command = new()
            {
                CommandText = "UPDATE images SET custom_name=@custom_name WHERE id=@id",
                Connection = Connection
            };

            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));

            // execute and close
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
