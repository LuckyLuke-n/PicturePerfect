using Avalonia.Media.Imaging;
using ImageMagick;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        /// Get the camera manufacturer and model.
        /// </summary>
        public string Camera => ExtractMetadata(MetadataDescriptions.CameraInfo);
        /// <summary>
        /// Get the ISO value set for this image.
        /// </summary>
        public string ISO => ExtractMetadata(MetadataDescriptions.ISO);
        /// <summary>
        /// Get the F-stop value for this image.
        /// </summary>
        public string FStop => ExtractMetadata(MetadataDescriptions.FStop);
        /// <summary>
        /// Get the exposure time for this image.
        /// </summary>
        public string ExposureTime => ExtractMetadata(MetadataDescriptions.ExposureTime);
        /// <summary>
        /// Get the exposure bias for this image.
        /// </summary>
        public string ExposureBias => ExtractMetadata(MetadataDescriptions.ExposureBias);
        /// <summary>
        /// Get the focal length for this image.
        /// </summary>
        public string FocalLength => ExtractMetadata(MetadataDescriptions.FocalLength);

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
        public static string[] OrfStrings => new string[] { ".orf", ".ORF" };
        public static string[] NefStrings => new string[] { ".nef", ".NEF" };
        public static string[] JpgStrings => new string[] { ".jpg", ".JPG" };
        public static string[] PngStrings => new string[] { ".png", ".PNG" };
        public static string[] TifStrings => new string[] { ".tif", ".TIF", ".tiff", ".TIFF" };

        // metadata directories
        /// <summary>
        /// Get the ExifDirectory for meta data tags of images. In PicutrePerfect this is currently supported only for .orf and .jpg files.
        /// </summary>
        private ExifSubIfdDirectory? SubIfdDirectory { get; set; } = null;

        /// <summary>
        /// Creates a new instance of the image file class.
        /// </summary>
        public ImageFile()
        {
        }

        /// <summary>
        /// Possible metadata to be extracted from the image files.
        /// </summary>
        private enum MetadataDescriptions
        {
            CameraInfo,
            FStop,
            ISO,
            ExposureTime,
            ExposureBias,
            FocalLength,
            DateTimeDigitized
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
        public static ImageFile NewFromDatabase(int id, string name, string customName, string subfolderName, string fileType, DateTime dateTaken, double size, string notes, Locations.Location location, Category category, SubCategory subCategory1, SubCategory subCategory2)
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
                Notes = notes,
                Location = location,
                Category = category,
                SubCategory1 = subCategory1,
                SubCategory2 = subCategory2
            };

            // store the metadata directory 
            // this can be null
            IReadOnlyList<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(imageFile.AbsolutePath);
            imageFile.SubIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            return imageFile;
        }

        /// <summary>
        /// Method to create an image file object from a path and save the properties to the database.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="subfolderName"></param>
        /// <returns>Returns the image file object.</returns>
        public static ImageFile NewToDatabase(string path, string subfolderName, Locations.Location location)
        {
            // create an image file from the path
            ImageFile imageFile = new();

            FileInfo fileInfo = new(path);
            imageFile.CustomName = fileInfo.Name;
            imageFile.Name = fileInfo.Name;
            imageFile.FileType = fileInfo.Extension;
            imageFile.Size = Math.Round(fileInfo.Length / 1000000.00, 3);

            imageFile.Subfolder = subfolderName;
            imageFile.Location = location; // this is either the default location "None" or a user selection.

            // store the metadata directory 
            // this can be null
            IReadOnlyList<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(path);
            imageFile.SubIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            // fix the date digitized bug
            if (imageFile.SubIfdDirectory != null && OrfStrings.Contains(imageFile.FileType))
            {
                // orf
                imageFile.DateTaken = DateTime.ParseExact(imageFile.SubIfdDirectory.Tags[7].Description, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture);
            }
            else if (imageFile.SubIfdDirectory != null && JpgStrings.Contains(imageFile.FileType))
            {
                // jpg
                try
                {
                    imageFile.DateTaken = DateTime.ParseExact(imageFile.SubIfdDirectory.Tags[14].Description, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture);
                }
                catch
                {
                    // datetime could not be parsed
                    imageFile.DateTaken = fileInfo.LastWriteTime; // Last write time is the creation date for un-edited files.
                }
                
            }
            else if (imageFile.SubIfdDirectory != null && TifStrings.Contains(imageFile.FileType))
            {
                // tiff
                imageFile.DateTaken = DateTime.ParseExact(imageFile.SubIfdDirectory.Tags[6].Description, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture);
            }
            else
            {
                imageFile.DateTaken = fileInfo.LastWriteTime; // Last write time is the creation date for un-edited files.
            }

            // copy the file to the image folder's subfolder
            string destination = Path.Combine(ThisApplication.ProjectFile.ImageFolder, imageFile.Subfolder, imageFile.Name);
            File.Copy(path, destination, true);

            // add to sqlite
            Database.AddImage(imageFile);

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
        /// Save the changed made to the notex property to the database.
        /// </summary>
        /// <param name="newNotes"></param>
        /// <returns>The updated image file object.</returns>
        public ImageFile CommitNotesChange(string newNotes)
        {
            Notes = newNotes;
            Database.SetNotes(imageFile: this);

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
            // set category object
            Category = category;
            Database.LinkImageToCategory(this, category);

            // reset subcategories, since links to subcategories were deleted in Database.LinkImageToCategory method
            SubCategory1 = new SubCategory();
            SubCategory2 = new SubCategory();

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

            if (OrfStrings.Contains(FileType) || NefStrings.Contains(FileType) || TifStrings.Contains(FileType))
            {
                MagickImage magickImage = new(AbsolutePath);
                bitmap = magickImage.ToBitmap();
            }
            else if (JpgStrings.Contains(FileType) || PngStrings.Contains(FileType))
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
            else if ((NefStrings.Contains(outputType) || OrfStrings.Contains(outputType)) & JpgStrings.Contains(FileType))
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
                    if (JpgStrings.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Jpg);
                    }
                    else if (PngStrings.Contains(outputType))
                    {
                        rawImage.Write(GetOutputFileName(), MagickFormat.Png);
                    }
                    else if (NefStrings.Contains(outputType))
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
        /// Method to delete this images from the database and the folder on the drive.
        /// </summary>
        public void DeleteFromDatabase()
        {
            Database.DeleteImage(this);
            File.Delete(AbsolutePath);
        }

        /// <summary>
        /// Method to open the image in an external image viewer.
        /// </summary>
        /// <param name="pathToExe"></param>
        public void OpenInExternalViewer(string pathToExe)
        {
            // copy the image to a temp folder
            string path = Path.Combine(ThisApplication.TempFolderPath, Name);
            System.IO.Directory.CreateDirectory(ThisApplication.TempFolderPath); // create folder if it does no exist
            File.Copy(sourceFileName: AbsolutePath, destFileName: path, overwrite: true);

            // open the file in the selected image viewer
            Process process = new();
            process.StartInfo.FileName = pathToExe;
            process.StartInfo.Arguments = path;
            process.Start();
        }

        /// <summary>
        /// Extract a metadata description from this image file.
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Returns the metadata as a string.</returns>
        /// <exception cref="ArgumentException"></exception>
        private string ExtractMetadata(MetadataDescriptions description)
        {
            string metadata;

            // check for file type
            if (SubIfdDirectory != null && OrfStrings.Contains(FileType))
            {
                // file is of type orf
                // check cases
                switch (description)
                {
                    case MetadataDescriptions.CameraInfo:
                        metadata = SubIfdDirectory.Tags[32].Description; // lens model as string
                        break;
                    case MetadataDescriptions.FStop:
                        metadata = SubIfdDirectory.Tags[1].Description; // e.g. f/8,0
                        break;
                    case MetadataDescriptions.ISO:
                        metadata = SubIfdDirectory.Tags[3].Description; // e.g.250
                        break;
                    case MetadataDescriptions.ExposureTime:
                        metadata = SubIfdDirectory.Tags[0].Description; // e.g. 1/500 sec
                        break;
                    case MetadataDescriptions.ExposureBias:
                        metadata = SubIfdDirectory.Tags[11].Description; // e.g. 38 0 EV;
                        break;
                    case MetadataDescriptions.FocalLength:
                        metadata = SubIfdDirectory.Tags[16].Description; // e.g. 38 mm;
                        break;
                    case MetadataDescriptions.DateTimeDigitized:
                        metadata = DateTime.ParseExact(SubIfdDirectory.Tags[7].Description, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture).ToString(); // 2021:06:15 09:42:21 to 15.06.2021 09:42:21
                        break;
                    default:
                        throw new ArgumentException("Error while extracting metadata.");
                        //break;
                }
            }
            else if (SubIfdDirectory != null && JpgStrings.Contains(FileType))
            {
                // file is of type jpg
                // check cases
                switch (description)
                {
                    case MetadataDescriptions.CameraInfo:
                        metadata = "unknown";
                        break;
                    case MetadataDescriptions.FStop:
                        metadata = SubIfdDirectory.Tags[1].Description; // e.g. f/8,0
                        break;
                    case MetadataDescriptions.ISO:
                        metadata = SubIfdDirectory.Tags[0].Description; // e.g.250
                        break;
                    case MetadataDescriptions.ExposureTime:
                        metadata = SubIfdDirectory.Tags[2].Description; // e.g. 1/500 sec
                        break;
                    case MetadataDescriptions.ExposureBias:
                        metadata = SubIfdDirectory.Tags[15].Description; // e.g. 38 0 EV;
                        break;
                    case MetadataDescriptions.FocalLength:
                        metadata = SubIfdDirectory.Tags[12].Description; // e.g. 38 mm;
                        break;
                    case MetadataDescriptions.DateTimeDigitized:
                        metadata = DateTime.ParseExact(SubIfdDirectory.Tags[14].Description, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture).ToString(); // 2021:06:15 09:42:21 to 15.06.2021 09:42:21
                        break;
                    default:
                        throw new ArgumentException("Error while extracting metadata.");
                        //break;
                }
            }
            else if (SubIfdDirectory != null && TifStrings.Contains(FileType))
            {
                // file is of type tif/tiff
                // this will not work with all tif files, the success depends on the source of the tif file
                // check cases
                switch (description)
                {
                    case MetadataDescriptions.CameraInfo:
                        metadata = SubIfdDirectory.Tags[25].Description; // lens model as string
                        break;
                    case MetadataDescriptions.FStop:
                        metadata = SubIfdDirectory.Tags[1].Description; // e.g. f/8,0
                        break;
                    case MetadataDescriptions.ISO:
                        metadata = SubIfdDirectory.Tags[3].Description; // e.g.250
                        break;
                    case MetadataDescriptions.ExposureTime:
                        metadata = SubIfdDirectory.Tags[0].Description; // e.g. 1/500 sec
                        break;
                    case MetadataDescriptions.ExposureBias:
                        metadata = SubIfdDirectory.Tags[7].Description; // e.g. 38 0 EV;
                        break;
                    case MetadataDescriptions.FocalLength:
                        metadata = SubIfdDirectory.Tags[12].Description; // e.g. 38 mm;
                        break;
                    case MetadataDescriptions.DateTimeDigitized:
                        metadata = DateTime.ParseExact(SubIfdDirectory.Tags[6].Description, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture).ToString(); // 2021:06:15 09:42:21 to 15.06.2021 09:42:21
                        break;
                    default:
                        throw new ArgumentException("Error while extracting metadata.");
                        //break;
                }
            }
            else
            {
                // image file is of type tif or nef
                // no metadata extractable yet
                metadata = "unknown";
            }

            // avoid null return --> display "not found"
            if (metadata == null)
            {
                metadata = "not found";
            }

            return metadata;
        }
    }
}
