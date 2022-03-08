using Avalonia.Media.Imaging;
using ImageMagick;
using System;
using System.IO;
using System.Linq;

namespace PicturePerfect.Models
{
    public class ImageFile
    {
        /// <summary>
        /// Get or set the custom name. This will not change the file name.
        /// Per default this equals the file name.
        /// </summary>
        public string CustomName { get; private set; }
        /// <summary>
        /// Get the filename. Per default this is the filename.
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the subfolder name located in .../images/
        /// </summary>
        public string Subfolder { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the file type.
        /// </summary>
        public string FileType { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the date when the image was taken.
        /// </summary>
        public DateTime DateTaken { get; private set; }
        /// <summary>
        /// Get or set the file size of this image.
        /// </summary>
        public double Size { get; private set; }
        /// <summary>
        /// Get or set the camera manufacturer and model.
        /// </summary>
        public string Camera { get; private set; } = string.Empty;
        /// <summary>
        /// Get or set the ISO value set for this image.
        /// </summary>
        public int ISO { get; private set; }
        /// <summary>
        /// Get or set the F-stop value for this image.
        /// It should be displayed as f/[value]
        /// </summary>
        public double FStop { get; private set; }
        /// <summary>
        /// Get or set the exposure time in milli-sec.
        /// </summary>
        public int ExposureTime { get; private set; }
        /// <summary>
        /// Get or set the exposure bias in steps for this image.
        /// </summary>
        public double ExposureBias { get; private set; }
        /// <summary>
        /// Get or set the focal length in mm for this image.
        /// </summary>
        public double FocalLength { get; private set; }

        /// <summary>
        /// Get or set the loacation where this image was taken.
        /// </summary>
        public Locations.Location Location { get; private set; } = new();
        /// <summary>
        /// Get or set the category for this window.
        /// </summary>
        public Category Category { get; private set; } = new();
        /// <summary>
        /// Get or set the sub category #1.
        /// </summary>
        public SubCategory SubCategory1 { get; private set; } = new();
        /// <summary>
        /// Get or set the sub category #2.
        /// </summary>
        public SubCategory SubCategory2 { get; private set; } = new();
        /// <summary>
        /// Get or set the notes for this image.
        /// </summary>
        public string Notes { get; private set; } = string.Empty;
        /// <summary>
        /// Get the image id. This is the id number from the sqlite database.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Get the absolute path to this image file.
        /// </summary>
        public string AbsolutePath => Path.Combine(ThisApplication.ProjectFile.ImageFolder, Subfolder, Name);

        // file type arrays
        private readonly string[] orf = { ".orf", ".ORF" };
        private readonly string[] nef = { ".nef", ".NEF" };
        private readonly string[] jpg = { ".jpg", ".JPG" };
        private readonly string[] png = { ".png", ".PNG" };


        /// <summary>
        /// Creates a new instance of the image file class.
        /// </summary>
        public ImageFile()
        {

        }

        /// <summary>
        /// Creates a new image file object with information from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="customName"></param>
        /// <param name="subfolderName"></param>
        /// <param name="fileType"></param>
        /// <param name="dateTaken"></param>
        /// <param name="size"></param>
        /// <param name="camera"></param>
        /// <param name="fStop"></param>
        /// <param name="iso"></param>
        /// <param name="exposureTime"></param>
        /// <param name="exposureBias"></param>
        /// <param name="focalLength"></param>
        /// <param name="notes"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile NewFromDatabase(int id, string name, string customName, string subfolderName, string fileType, DateTime dateTaken, double size, string camera, double fStop, int iso, int exposureTime, double exposureBias, double focalLength, string notes, Locations.Location location, Category category, SubCategory subCategory1, SubCategory subCategory2)
        {
            ImageFile imageFile = new()
            {
                Id = id,
                Name = name,
                CustomName = customName,
                Subfolder = subfolderName,
                FileType = fileType,
                DateTaken = dateTaken,
                Size = size,
                Camera = camera,
                ISO = iso,
                FStop = fStop,
                ExposureTime = exposureTime,
                ExposureBias = exposureBias,
                FocalLength = focalLength,
                Notes = notes,
                Location = location,
                Category = category,
                SubCategory1 = subCategory1,
                SubCategory2 = subCategory2
            };

            return imageFile;
        }

        /// <summary>
        /// Method to create an image file object from a path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="subfolderName"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile NewFromPath(string path, string subfolderName)
        {
            ImageFile imageFile = new();

            FileInfo fileInfo = new(path);
            imageFile.CustomName = fileInfo.Name;
            imageFile.Name = fileInfo.Name;
            imageFile.Subfolder = subfolderName;
            imageFile.FileType = fileInfo.Extension;
            imageFile.DateTaken = fileInfo.LastWriteTime; // Last write time is the creation date for un.edited files. This is a work around since it was not possible to read the create date from exifdirectory.
            imageFile.Size = Math.Round(fileInfo.Length / 1000000.00, 3);

            // create the entry
            CreateDatabaseEntry(imageFile, path);

            return imageFile;
        }

        /// <summary>
        /// Method to load the image file object from the database by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile LoadById(int id)
        {
            return Database.LoadImageFileById(id);
        }

        /// <summary>
        /// Method to copy the files and create a sqlite entry for this image.
        /// </summary>
        private static void CreateDatabaseEntry(ImageFile imageFile, string path)
        {
            // copy the file to the image folder's subfolder
            string destination = Path.Combine(ThisApplication.ProjectFile.ImageFolder, imageFile.Subfolder, imageFile.Name);
            File.Copy(path, destination, true);

            // add to sqlite
            Database.AddImage(imageFile);
        }

        /// <summary>
        /// Save changes made to the image meta data.
        /// </summary>
        /// <param name="newName"></param>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitCustomFileNameChange(string newName)
        {
            CustomName = newName;
            Database.SetCustomName(imageFile: this);

            return this;
        }

        /// <summary>
        /// Save changes made to the image location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitLocationChange(Locations.Location location)
        {
            Location = location;
            Database.LinkImageToLocation(this, location);

            return this;
        }

        /// <summary>
        /// Save changes made to the image category.
        /// </summary>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitCategoryChange(Category category)
        {
            Category = category;
            Database.LinkImageToCategory(this, category);

            return this;
        }

        /// <summary>
        /// Save changes made to the image sub category 1.
        /// </summary>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitSubCategory1Change(SubCategory newSubCategory, SubCategory oldSubCategory)
        {
            oldSubCategory = SubCategory1;
            SubCategory1 = newSubCategory;
            Database.LinkImageToSubCategory(this, newSubCategory, oldSubCategory);

            return this;
        }

        /// <summary>
        /// Save changes made to the image sub category 2.
        /// </summary>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitSubCategory2Change(SubCategory newSubCategory, SubCategory oldSubCategory)
        {
            oldSubCategory = SubCategory2;
            SubCategory2 = newSubCategory;
            Database.LinkImageToSubCategory(this, newSubCategory, oldSubCategory);

            return this;
        }



        /// <summary>
        /// Method to get this image to a bitmap.
        /// </summary>
        /// <returns>Returns the bitmap.</returns>
        public Bitmap ToBitmap()
        {
            Bitmap bitmap;

            if (orf.Contains(FileType) || nef.Contains(FileType))
            {
                MagickImage magickImage = new(AbsolutePath);
                bitmap = magickImage.ToBitmap();
            }
            else if (jpg.Contains(FileType) || png.Contains(FileType))
            {
                bitmap = BitmapValueConverter.Convert(AbsolutePath);
            }
            else
            {
                bitmap = ThisApplication.PlaceholderImage;
            }
            return bitmap;
        }

        /// <summary>
        /// Method to convert a raw file image to jpg-format. The image will be savd to the specified path.
        /// </summary>
        /// <param name="toFolder"></param>
        /// <param name="outputType"></param>
        /// <returns>Returns true if conversion was successful.</returns>
        public bool Export(string toFolder, string outputType = ".jpg")
        {
            // create new file name with new extension based on custom or initial name
            string GetOutputFileName()
            {
                string newFileName = string.Empty;

                // check custom or initial name
                if (Name != CustomName)
                {
                    newFileName = $"{CustomName.Split(".").First()}{outputType}";
                }
                else
                {
                    newFileName = $"{Name.Split(".").First()}{outputType}";
                }

                string fileName = Path.Combine(paths: new string[] { toFolder, newFileName });

                return fileName;
            }

            if (outputType == FileType || outputType.ToUpper() == FileType)
            {
                // only copy the file
                string outputFileName = Path.Combine(toFolder, Name);
                File.Copy(AbsolutePath, outputFileName, true);
                return true;
            }
            else if ((nef.Contains(outputType) || orf.Contains(outputType)) & jpg.Contains(FileType))
            {
                // jpg will not be converted to raw on output!
                return false;
            }
            else
            {
                // type is some sort of raw file that will be converted
                // decode the image using the MagickImage library
                using (MagickImage rawImage = new(this.AbsolutePath))
                {
                    if (jpg.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Jpg);
                    }
                    else if (png.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Png);
                    }
                    else if (nef.Contains(outputType))
                    {
                        // no conversion
                        // rawImage.Write(GetOutputFileName(), MagickFormat.Nef);
                    }
                    else
                    {
                        // no conversion
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Method to delete this images from the database.
        /// </summary>
        public void DeleteFromDatabase()
        {

        }
    }
}
