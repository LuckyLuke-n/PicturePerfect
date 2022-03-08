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
            GeoTag,
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
                        "CREATE TABLE images_categories (id INTEGER PRIMARY KEY, image_id INTEGER, category_id INTEGER)",
                        "CREATE TABLE images_subcategories (id INTEGER PRIMARY KEY, image_id INTEGER, subcategory_id INTEGER)",
                        "CREATE TABLE images_locations (id INTEGER PRIMARY KEY, image_id INTEGER, location_id INTEGER)"};

            // run queries against database
            foreach (string query in queries)
            {
                var command = new SqliteCommand(query, Connection);
                command.ExecuteNonQuery();
            }

            string[] defaultQueries = { "INSERT INTO locations (name, geo_tag, notes) VALUES ('None', '', '')",
                        "INSERT INTO categories (name, notes) VALUES ('All', '')",
                        "INSERT INTO categories (name, notes) VALUES ('None', '')",
                        "INSERT INTO subcategories (name, notes) VALUES ('None', '')" };

            // run queries against database
            foreach (string query in defaultQueries)
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

            void AddToImages()
            {
                // new command
                SqliteCommand command = new()
                {
                    CommandText = "INSERT INTO images ( custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes) " +
                        " VALUES (@custom_name, @name, @subfolder, @file_type, @date_taken, @size, @camera, @iso, @fstop, @exposure_time, @exposure_bias, @focal_length, @notes)",
                    Connection = Connection
                };

                // add all with value, only works if each column is unique, which should always be the case
                paramters.ForEach(parameter => command.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));
                command.ExecuteNonQuery();
            }

            void LinkToCategoryAndLocation()
            {
                // image id of this image
                SqliteCommand commandImageId = new()
                {
                    CommandText = "SELECT MAX(id) FROM images",
                    Connection = Connection
                };
                // execute reader
                SqliteDataReader reader = commandImageId.ExecuteReader();
                reader.Read();
                int imageId = reader.GetInt32(0);

                // link to category "None"
                SqliteCommand commandCategory = new()
                {
                    CommandText = "INSERT INTO images_categories (image_id, category_id) VALUES (@image_id, @category_id)",
                    Connection = Connection
                };
                commandCategory.Parameters.AddWithValue("@image_id", imageId);
                commandCategory.Parameters.AddWithValue("@category_id", 2);
                commandCategory.ExecuteNonQuery();

                // link to location "None"
                SqliteCommand commandLocation = new()
                {
                    CommandText = "INSERT INTO images_locations (image_id, location_id) VALUES (@image_id, @location_id)",
                    Connection = Connection
                };
                commandLocation.Parameters.AddWithValue("@image_id", imageId);
                commandLocation.Parameters.AddWithValue("@location_id", 1);
                commandLocation.ExecuteNonQuery();
            }

            AddToImages();
            LinkToCategoryAndLocation();

            // close connection
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
        /// Method to link an image to a category. This method deletes the old links with categories and subcategories.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="category"></param>
        public static void LinkImageToCategory(ImageFile image, Category category)
        {
            //  values and parameters
            List<string> paramters = new() { "@image_id", "@category_id" };
            object[] values = { image.Id, category.Id };

            string[] commandsDelete = { "DELETE FROM images_categories WHERE image_id=@image_id", "DELETE FROM images_subcategories WHERE image_id=@image_id" };

            // new command
            Connection.Open();

            // delete old links
            foreach (string commandText in commandsDelete)
            {
                SqliteCommand commandDelete = new()
                {
                    CommandText = commandText,
                    Connection = Connection
                };
                commandDelete.Parameters.AddWithValue("@image_id", image.Id);
                commandDelete.ExecuteNonQuery();
            }

            // insert new row linking the image and the location
            SqliteCommand commandNew = new()
            {
                CommandText = "INSERT INTO images_categories (image_id, category_id) " +
                    " VALUES (@image_id, @category_id)",
                Connection = Connection
            };
            // add all with value, only works if each column is unique, which should always be the case
            paramters.ForEach(parameter => commandNew.Parameters.AddWithValue(parameter, values[paramters.IndexOf(parameter)]));
            commandNew.ExecuteNonQuery();

            // close connection
            Connection.Close();
        }

        /// <summary>
        /// Method to link an image to a subcategory.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="oldSubCategory"></param>
        /// <param name="newSubCategory"></param>
        public static void LinkImageToSubCategory(ImageFile image, SubCategory newSubCategory, SubCategory oldSubCategory)
        {
            string[] commandsDelete = { "DELETE FROM images_subcategories WHERE image_id=@image_id AND subcategory_id=@oldsubcategory_id" };

            // new command
            Connection.Open();

            if (oldSubCategory.Id != 0)
            {
                // delete old link
                foreach (string commandText in commandsDelete)
                {
                    SqliteCommand commandDelete = new()
                    {
                        CommandText = commandText,
                        Connection = Connection
                    };
                    commandDelete.Parameters.AddWithValue("@image_id", image.Id);
                    commandDelete.Parameters.AddWithValue("@oldsubcategory_id", oldSubCategory.Id);
                    commandDelete.ExecuteNonQuery();
                }
            }

            // insert new row linking the image and the location
            SqliteCommand commandNew = new()
            {
                CommandText = "INSERT INTO images_subcategories (image_id, subcategory_id) " +
                    " VALUES (@image_id, @newsubcategory_id)",
                Connection = Connection
            };
            // add all with value, only works if each column is unique, which should always be the case
            commandNew.Parameters.AddWithValue("@image_id", image.Id);
            commandNew.Parameters.AddWithValue("@newsubcategory_id", newSubCategory.Id);
            commandNew.ExecuteNonQuery();

            // close connection
            Connection.Close();
        }

        /// <summary>
        /// Method to load an image file by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile LoadImageFileById(int id)
        {
            ImageFile imageFile = new();

            Connection.Open();

            string commandTextImage = "SELECT id, custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes FROM images WHERE id=@id";
            SqliteCommand commandImage = new(commandTextImage, Connection);
            commandImage.Parameters.AddWithValue("@id", id);
            SqliteDataReader readerImage = commandImage.ExecuteReader();

            if (readerImage.HasRows)
            {
                // only one entry as a result
                readerImage.Read();

                string customName = readerImage.GetString((int)TableImagesOrdinals.CustomName);
                string name = readerImage.GetString((int)TableImagesOrdinals.Name);
                string subfolderName = readerImage.GetString((int)TableImagesOrdinals.Subfolder);
                string fileType = readerImage.GetString((int)TableImagesOrdinals.FileType);
                DateTime dateTaken = DateTime.Parse(readerImage.GetString((int)TableImagesOrdinals.DateTaken));
                double size = readerImage.GetDouble((int)TableImagesOrdinals.Size);
                string camera = readerImage.GetString((int)TableImagesOrdinals.Camera);
                int iso = readerImage.GetInt32((int)TableImagesOrdinals.ISO);
                double fStop = readerImage.GetDouble((int)TableImagesOrdinals.FStop);
                int exposureTime = readerImage.GetInt32((int)TableImagesOrdinals.ExposureTime);
                double exposureBias = readerImage.GetDouble((int)TableImagesOrdinals.ExposureBias);
                double focalLength = readerImage.GetDouble((int)TableImagesOrdinals.FocalLength);
                string notes = readerImage.GetString((int)TableImagesOrdinals.Notes);

                Locations.Location location = GetLocation(id);
                Category category = GetCategory(id);
                SubCategory[] subCategories = GetSubCategories(id);

                imageFile = ImageFile.NewFromDatabase(id, name, customName, subfolderName, fileType, dateTaken, size, camera, fStop, iso, exposureTime, exposureBias, focalLength, notes, location, category, subCategories[0], subCategories[1]);
            }

            Connection.Close();

            return imageFile;
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
                Category category = GetCategory(id);
                SubCategory[] subCategories = GetSubCategories(id);

                ImageFile imageFile = ImageFile.NewFromDatabase(id, name, customName, subfolderName, fileType, dateTaken, size, camera, fStop, iso, exposureTime, exposureBias, focalLength, notes, location, category, subCategories[0], subCategories[1]);

                list.Add(imageFile);
            }

            // close connection
            Connection.Close();

            return list;
        }

        /// <summary>
        /// Method to load all image file data for files with a specific category assignment.
        /// </summary>
        /// /// <param name="category"></param>
        /// <returns>Returns the list of image files.</returns>
        public static List<ImageFile> LoadImageFilesByCategory(Category category)
        {
            List<ImageFile> list = new();

            if (category.Id == 1)
            {
                // load all
                list = LoadAllImageFiles();
            }
            else
            {
                // specific category
                string commandTextIds = "SELECT image_id FROM images_categories WHERE category_id=@category_id";

                Connection.Open();

                SqliteCommand commandIds = new(commandTextIds, Connection);
                commandIds.Parameters.AddWithValue("category_id", category.Id);
                SqliteDataReader readerId = commandIds.ExecuteReader();

                if (readerId.HasRows)
                {
                    while (readerId.Read())
                    {
                        // get id and corresponding image
                        int imageId = readerId.GetInt32(0);

                        string commandTextImage = "SELECT id, custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes FROM images WHERE id=@id ORDER BY date_taken ASC";
                        SqliteCommand commandImage = new(commandTextImage, Connection);
                        commandImage.Parameters.AddWithValue("@id", imageId);
                        SqliteDataReader readerImage = commandImage.ExecuteReader();

                        // only one entry as a result
                        readerImage.Read();

                        int id = readerImage.GetInt32((int)TableImagesOrdinals.Id);
                        string customName = readerImage.GetString((int)TableImagesOrdinals.CustomName);
                        string name = readerImage.GetString((int)TableImagesOrdinals.Name);
                        string subfolderName = readerImage.GetString((int)TableImagesOrdinals.Subfolder);
                        string fileType = readerImage.GetString((int)TableImagesOrdinals.FileType);
                        DateTime dateTaken = DateTime.Parse(readerImage.GetString((int)TableImagesOrdinals.DateTaken));
                        double size = readerImage.GetDouble((int)TableImagesOrdinals.Size);
                        string camera = readerImage.GetString((int)TableImagesOrdinals.Camera);
                        int iso = readerImage.GetInt32((int)TableImagesOrdinals.ISO);
                        double fStop = readerImage.GetDouble((int)TableImagesOrdinals.FStop);
                        int exposureTime = readerImage.GetInt32((int)TableImagesOrdinals.ExposureTime);
                        double exposureBias = readerImage.GetDouble((int)TableImagesOrdinals.ExposureBias);
                        double focalLength = readerImage.GetDouble((int)TableImagesOrdinals.FocalLength);
                        string notes = readerImage.GetString((int)TableImagesOrdinals.Notes);

                        Locations.Location location = GetLocation(id);
                        SubCategory[] subCategories = GetSubCategories(id);

                        ImageFile imageFile = ImageFile.NewFromDatabase(id, name, customName, subfolderName, fileType, dateTaken, size, camera, fStop, iso, exposureTime, exposureBias, focalLength, notes, location, category, subCategories[0], subCategories[1]);

                        list.Add(imageFile);
                    }
                }

                Connection.Close();
            }

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

            // specific category
            string commandTextIds = "SELECT image_id FROM images_subcategories WHERE subcategory_id=@subcategory_id";

            Connection.Open();

            SqliteCommand commandIds = new(commandTextIds, Connection);
            commandIds.Parameters.AddWithValue("subcategory_id", subCategory.Id);
            SqliteDataReader readerId = commandIds.ExecuteReader();

            if (readerId.HasRows)
            {
                while (readerId.Read())
                {
                    // get id and corresponding image
                    int imageId = readerId.GetInt32(0);

                    string commandTextImage = "SELECT id, custom_name, name, subfolder, file_type, date_taken, size, camera, iso, fstop, exposure_time, exposure_bias, focal_length, notes FROM images WHERE id=@id ORDER BY date_taken ASC";
                    SqliteCommand commandImage = new(commandTextImage, Connection);
                    commandImage.Parameters.AddWithValue("@id", imageId);
                    SqliteDataReader readerImage = commandImage.ExecuteReader();

                    // only one entry as a result
                    readerImage.Read();

                    int id = readerImage.GetInt32((int)TableImagesOrdinals.Id);
                    string customName = readerImage.GetString((int)TableImagesOrdinals.CustomName);
                    string name = readerImage.GetString((int)TableImagesOrdinals.Name);
                    string subfolderName = readerImage.GetString((int)TableImagesOrdinals.Subfolder);
                    string fileType = readerImage.GetString((int)TableImagesOrdinals.FileType);
                    DateTime dateTaken = DateTime.Parse(readerImage.GetString((int)TableImagesOrdinals.DateTaken));
                    double size = readerImage.GetDouble((int)TableImagesOrdinals.Size);
                    string camera = readerImage.GetString((int)TableImagesOrdinals.Camera);
                    int iso = readerImage.GetInt32((int)TableImagesOrdinals.ISO);
                    double fStop = readerImage.GetDouble((int)TableImagesOrdinals.FStop);
                    int exposureTime = readerImage.GetInt32((int)TableImagesOrdinals.ExposureTime);
                    double exposureBias = readerImage.GetDouble((int)TableImagesOrdinals.ExposureBias);
                    double focalLength = readerImage.GetDouble((int)TableImagesOrdinals.FocalLength);
                    string notes = readerImage.GetString((int)TableImagesOrdinals.Notes);

                    Locations.Location location = GetLocation(id);
                    Category category = GetCategory(id);
                    SubCategory[] subCategories = GetSubCategories(id);

                    ImageFile imageFile = ImageFile.NewFromDatabase(id, name, customName, subfolderName, fileType, dateTaken, size, camera, fStop, iso, exposureTime, exposureBias, focalLength, notes, location, category, subCategories[0], subCategories[1]);

                    list.Add(imageFile);
                }
            }

            Connection.Close();
            
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
                    

                    int identifier = readerLocation.GetInt32(0);
                    string name = readerLocation.GetString(1);
                    string geoTag = readerLocation.GetString(2);
                    string notes = readerLocation.GetString(3);

                    location = Locations.Location.NewFromDatabase(identifier, name, geoTag, notes);
                }
            }

            return location;
        }

        /// <summary>
        /// Method to get the category of a specific image file.
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns>Returns the category object.</returns>
        private static Category GetCategory(int imageId)
        {
            Category category = new();

            // query subcategory ids
            string commandText = @"SELECT category_id FROM images_categories WHERE image_id=@image_id";

            // Connect to the Sqlite database
            SqliteCommand command = new(commandText, Connection);
            command.Parameters.AddWithValue("@image_id", imageId);

            // Sqlite data reader
            // this reader will not contain elements if the image has no location assigned
            SqliteDataReader reader = command.ExecuteReader();
            // step through the reader containing the subcategory ids           
            if (reader.HasRows)
            {
                // reader will only have one item since a image can only have one category
                reader.Read();
                int id = reader.GetInt32(0); // index 0 is the location id

                string commandTextCategory = @"SELECT id, name, notes FROM categories WHERE id=@id";
                SqliteCommand commandLocation = new(commandTextCategory, Connection);
                commandLocation.Parameters.AddWithValue("@id", id);
                // call the reader for the location by using the category id
                SqliteDataReader readerLocation = commandLocation.ExecuteReader();

                // reader for the category command will only contain one item, since id is unique
                if (readerLocation.HasRows)
                {
                    readerLocation.Read();
                    // set the properties of the category object
                    int identifier = readerLocation.GetInt32(0);
                    string name = readerLocation.GetString(1);
                    string notes = readerLocation.GetString(2);

                    category = new()
                    {
                        Id = identifier,
                        Name = name,
                        Notes = notes
                    };
                }
            }

            return category;
        }

        /// <summary>
        /// Method to get the assigned subcategories for a specific image file.
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="number"></param>
        /// <returns>Returns an array containing the two subcategories. The first element in subcategory 1. The second element is subcategory 2.</returns>
        private static SubCategory[] GetSubCategories(int imageId)
        {
            SubCategory[] subCategories = { new SubCategory(), new SubCategory() };

            // query subcategory ids
            string commandText = @"SELECT subcategory_id FROM images_subcategories WHERE image_id=@image_id";

            // Connect to the Sqlite database
            SqliteCommand command = new(commandText, Connection);
            command.Parameters.AddWithValue("@image_id", imageId);

            // Sqlite data reader
            // this reader will not contain elements if the image has no subcategory assigned
            SqliteDataReader reader = command.ExecuteReader();
            // step through the reader containing the subcategory ids           
            if (reader.HasRows)
            {
                // reader will only have two item since a image can only have two subcategories
                int row = 0;
                while (reader.Read())
                {
                    int id = reader.GetInt32(0); // index 0 is the subcategory id

                    string commandTextCategory = @"SELECT id, name, notes FROM subcategories WHERE id=@id";
                    SqliteCommand commandLocation = new(commandTextCategory, Connection);
                    commandLocation.Parameters.AddWithValue("@id", id);
                    // call the reader for the location by using the category id
                    SqliteDataReader readerLocation = commandLocation.ExecuteReader();

                    // reader for the subcategory command will only contain one item, since id is unique
                    if (readerLocation.HasRows)
                    {
                        readerLocation.Read();                     
                        // set the properties of the category object
                        int identifier = readerLocation.GetInt32(0);
                        string name = readerLocation.GetString(1);
                        string notes = readerLocation.GetString(2);

                        subCategories[row].Id = identifier;
                        subCategories[row].Name = name;
                        subCategories[row].Notes = notes;
                    }
                    row++;
                }
            }

            return subCategories;
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
        private static List<SubCategory> LoadSubcategories(Category category)
        {
            List<SubCategory> list = new();

            // "all"
            if (category.Id == 1)
            {
                // "All"
            }
            // "none"
            else if(category.Id == 2)
            {
                // "None" -->no subcategories
                /*
                string commandText = @"SELECT * FROM subcategories ORDER BY name ASC";

                // Connect to the Sqlite database
                Connection.Open();

                SqliteCommand command = new(commandText, Connection);
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SubCategory subCategory = new()
                        {
                            Id = reader.GetInt32((int)TableSubCategoriesOrdinals.Id),
                            Name = reader.GetString((int)TableSubCategoriesOrdinals.Name),
                            Notes = reader.GetString((int)TableSubCategoriesOrdinals.Notes)
                        };
                        list.Add(subCategory);
                    }
                }

                // close connection
                Connection.Close();
                */
            }
            // any custom category
            else
            {
                // category selected
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

                        string commandTextSubCategory = @"SELECT id, name, notes FROM subcategories WHERE id=@id ORDER BY name ASC";
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
            }

            return list;
        }

        /// <summary>
        /// Method to return all locations stored in the database.
        /// </summary>
        /// <returns>REturns the locations object.</returns>
        public static List<Locations.Location> LoadAllLocations()
        {
            List<Locations.Location> locations = new();

            string commandText = @"SELECT * FROM locations ORDER BY name ASC";

            // Connect to the Sqlite database
            Connection.Open();
            SqliteCommand command = new(commandText, Connection);

            // execute reader
            SqliteDataReader reader = command.ExecuteReader();

            // step through reader
            while (reader.Read())
            {
                int id = reader.GetInt32((int)TableLocationsOrdinals.Id);
                string name = reader.GetString((int)TableLocationsOrdinals.Name);
                string geoTag = reader.GetString((int)TableLocationsOrdinals.GeoTag);
                string notes = reader.GetString((int)TableLocationsOrdinals.Notes);

                Locations.Location location = Locations.Location.NewFromDatabase(id, name, geoTag, notes);

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
