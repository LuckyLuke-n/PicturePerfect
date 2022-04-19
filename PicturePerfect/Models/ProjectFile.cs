using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PicturePerfect.Models
{
    internal class ProjectFile
    {
        #region General
        /// <summary>
        /// Get the project name.
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;
        /// <summary>
        /// Get the name of the project owner.
        /// </summary>
        public string ProjectOwner { get; private set; } = string.Empty;
        public DateTime CreationDate { get; private set; } = DateTime.Now;

        private string notes = string.Empty;
        /// <summary>
        /// Get or set the notes for this project. The change will be saved to the project file.
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set { notes = value; Save(); }
        }

        /// <summary>
        /// Get the absolute path to the project file.
        /// </summary>
        public string ProjectFilePath { get; private set; } = string.Empty;
        /// <summary>
        /// Get the absolute path to the image folder.
        /// </summary>
        public string ImageFolder { get; private set; } = string.Empty;
        /// <summary>
        /// Get the absolute path to the database sqlite file.
        /// </summary>
        public string DatabasePath { get; private set; } = string.Empty;
        /// <summary>
        /// Get the release number of the application this project file was last edited with.
        /// </summary>
        private string Release { get; set; } = string.Empty;
        #endregion

        #region Theme
        private string darkColor = ThisApplication.DarkColorDefault;
        private string mediumColor = ThisApplication.MediumColorDefault;
        private string lightColor = ThisApplication.LightColorDefault;
        private string lightFontColor = ThisApplication.LightFontColorDefault;
        private string darkContrastColor = ThisApplication.DarkContrastColorDefault;
        /// <summary>
        /// Get or set the value for the dark theme color.
        /// </summary>
        public string DarkColor
        {
            get { return darkColor; }
            set { darkColor = value; }
        }
        /// <summary>
        /// Get or set the value for the medium theme color.
        /// </summary>
        public string MediumColor
        {
            get { return mediumColor; }
            set { mediumColor = value;}
        }
        /// <summary>
        /// Get or set the value for the light theme color.
        /// </summary>
        public string LightColor
        {
            get { return lightColor; }
            set { lightColor = value; }
        }
        /// <summary>
        /// Get or set the value for the light font theme color.
        /// </summary>
        public string LightFontColor
        {
            get { return lightFontColor; }
            set { lightFontColor = value; }
        }
        /// <summary>
        /// Get or set the value for the dark contrast theme color.
        /// </summary>
        public string DarkContrastColor
        {
            get { return darkContrastColor; }
            set { darkContrastColor = value; }
        }
        #endregion

        #region Favorite images
        private int favorite1Id = 0;
        private int favorite2Id = 0;
        private int favorite3Id = 0;
        private int favorite4Id = 0;
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite1Id
        {
            get { return favorite1Id; }
            set { favorite1Id = value; Save(); }
        }
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite2Id
        {
            get { return favorite2Id; }
            set { favorite2Id = value; Save(); }
        }
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite3Id
        {
            get { return favorite3Id; }
            set { favorite3Id = value; Save(); }
        }
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite4Id
        {
            get { return favorite4Id; }
            set { favorite4Id = value; Save(); }
        }
        #endregion

        #region Settings
        private bool useSeparator = false;
        /// <summary>
        /// Get or set the use separator property.
        /// </summary>
        public bool UseSeparator
        {
            get { return useSeparator; }
            set { useSeparator = value; Save(); }
        }

        private string? separator = null;
        /// <summary>
        /// Get or set the seprator character for folder naming.
        /// </summary>
        public string? Separator
        {
            get { return separator; }
            set { separator = value; Save(); }
        }

        private bool nefFilesChecked = false;
        /// <summary>
        /// Get or set the raw files property. The change will be saved to the project file.
        /// </summary>
        public bool NefFilesChecked
        {
            get { return nefFilesChecked; }
            set { nefFilesChecked = value; Save(); }
        }

        private bool orfFilesChecked = false;
        /// <summary>
        /// Get or set the orf files property. The change will be saved to the project file.
        /// </summary>
        public bool OrfFilesChecked
        {
            get { return orfFilesChecked; }
            set { orfFilesChecked = value; Save(); }
        }

        private bool jpgFilesChecked = false;
        /// <summary>
        /// Get or set the jpg files property. The change will be saved to the project file.
        /// </summary>
        public bool JpgFilesChecked
        {
            get { return jpgFilesChecked; }
            set { jpgFilesChecked = value; Save(); }
        }

        private bool pngFilesChecked = false;
        /// <summary>
        /// Get or set the png files property. The change will be saved to the project file.
        /// </summary>
        public bool PngFilesChecked
        {
            get { return pngFilesChecked; }
            set { pngFilesChecked = value; Save(); }
        }

        private bool tifFilesChecked  = false;
        /// <summary>
        /// Get or set the rif files property. The change will be saved to the project file.
        /// </summary>
        public bool TifFilesChecked
        {
            get { return tifFilesChecked; }
            set { tifFilesChecked = value; Save(); }
        }

        private bool imageViewFullScreenChecked = false;
        /// <summary>
        /// Get or set the behavior for the image view window.
        /// </summary>
        public bool ImageViewFullScreenChecked
        {
            get { return imageViewFullScreenChecked; }
            set { imageViewFullScreenChecked = value; Save(); }
        }

        private string pathToExternalViewer = "";
        /// <summary>
        /// Get or set the path to the external image viewer.
        /// </summary>
        public string PathToExternalViewer
        {
            get { return pathToExternalViewer; }
            set { pathToExternalViewer = value; Save(); }
        }

        private bool autoBackupChecked = true;
        /// <summary>
        /// Get or set weather the auto backup shall be performed.
        /// </summary>
        public bool AutoBackupChecked
        {
            get { return autoBackupChecked; }
            set { autoBackupChecked = value; Save(); }
        }
        #endregion

        /// <summary>
        /// Creates a new instance of the class project file.
        /// This json file is used to store all relevant setings for a project.
        /// The file type is .ppp (PicturePerfectPoject-File)
        /// </summary>
        public ProjectFile()
        {

        }

        /// <summary>
        /// Method to create a new project file. The folders and subfolders for the project are created.
        /// The file is saved to the selected folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns>Return the project file.</returns>
        public static ProjectFile New(string path)
        {
            // new project file object
            ProjectFile file = new()
            {
                Release = ThisApplication.ApplicationVersion,
                ProjectName = new DirectoryInfo(path).Name,
                ProjectFilePath = Path.Combine(path, new DirectoryInfo(path).Name + ".ppp"),
                ProjectOwner = Environment.UserName,
                CreationDate = DateTime.Now,
                ImageFolder = Path.Combine(path, "images"),
                DatabasePath = Path.Combine(path, "sqlite", "database.sqlite"),
                OrfFilesChecked = true,
                NefFilesChecked = true,
                ImageViewFullScreenChecked = false,
                PathToExternalViewer = "Select a file"
            };

            // create basic folders
            Directory.CreateDirectory(new FileInfo(file.DatabasePath).DirectoryName);
            Directory.CreateDirectory(file.ImageFolder);

            // save object to json file
            Dictionary<string, string?> dictionary = file.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(file)?.ToString());
            string jsonString = JsonConvert.SerializeObject(dictionary);
            File.WriteAllText(file.ProjectFilePath, jsonString);

            return file;
        }

        /// <summary>
        /// Method to load the project file. The file will be resaved as a dictionary.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the project file object.</returns>
        public static ProjectFile Load(string path)
        {
            // load current file
            string jsonString = File.ReadAllText(path);
            //ProjectFile file = JsonConvert.DeserializeObject<ProjectFile>(jsonString);
            Dictionary<string, string?> projectFile = JsonConvert.DeserializeObject<Dictionary<string, string?>>(jsonString);

            // create new file object and carry over the information
            // this avoids compatibility issues in case the properties of this class are changed
            // removing properties is no problem, adding will cause a crash --> therfor CheckSomePropertyException()

            // catch errors for possibly changed properties
            // set default value if error occures
            bool CatchNefFilesCheckedException()
            {
                bool value;
                try { value = bool.Parse(projectFile["NefFilesChecked"]); }
                catch { value = false; }

                return value;
            }
            bool CheckOrfFilesCheckedException()
            {
                bool value;
                try { value = bool.Parse(projectFile["OrfFilesChecked"]); }
                catch { value = false; }

                return value;
            }
            bool CheckJpgFilesCheckedException()
            {
                bool value;
                try { value = bool.Parse(projectFile["JpgFilesChecked"]); }
                catch { value = false; }

                return value;
            }
            bool CheckPngFilesCheckedException()
            {
                bool value;
                try { value = bool.Parse(projectFile["PngFilesChecked"]); }
                catch { value = false; }

                return value;
            }
            bool CheckImageViewFullScreenChecked()
            {
                bool value;
                try { value = bool.Parse(projectFile["ImageViewFullScreenChecked"]); }
                catch { value = false; }

                return value;
            }
            bool CheckTifFilesCheckedException()
            {
                bool value;
                try { value = bool.Parse(projectFile["TifFilesChecked"]); }
                catch { value = false; }

                return value;
            }
            string CheckPathToExternalViewer()
            {
                string value;
                try { value = projectFile["PathToExternalViewer"]; }
                catch { value = "Select a file"; }

                return value;
            }
            bool CheckAutoBackupCheckedException()
            {
                bool value;
                try { value = bool.Parse(projectFile["AutoBackupChecked"]); }
                catch { value = false; }

                return value;
            }

            // update project file
            ProjectFile newFile = new()
            {
                Release = ThisApplication.ApplicationVersion,
                ProjectName = projectFile["ProjectName"],
                ProjectFilePath = projectFile["ProjectFilePath"],
                ProjectOwner = projectFile["ProjectOwner"],
                CreationDate = DateTime.Parse(projectFile["CreationDate"]),
                Notes = projectFile["Notes"],
                ImageFolder = Path.Combine(new FileInfo(path).DirectoryName, "images"),
                DatabasePath = Path.Combine(new FileInfo(path).DirectoryName, "sqlite", "database.sqlite"),

                // settings
                NefFilesChecked = CatchNefFilesCheckedException(),
                OrfFilesChecked = CheckOrfFilesCheckedException(),
                JpgFilesChecked = CheckJpgFilesCheckedException(),
                PngFilesChecked = CheckPngFilesCheckedException(),
                TifFilesChecked = CheckTifFilesCheckedException(),
                ImageViewFullScreenChecked = CheckImageViewFullScreenChecked(),
                PathToExternalViewer = CheckPathToExternalViewer(),
                AutoBackupChecked = CheckAutoBackupCheckedException(),
                UseSeparator = bool.Parse(projectFile["UseSeparator"]),
                Separator = projectFile["Separator"],

                // favorites
                Favorite1Id = Convert.ToInt32(projectFile["Favorite1Id"]),
                Favorite2Id = Convert.ToInt32(projectFile["Favorite2Id"]),
                Favorite3Id = Convert.ToInt32(projectFile["Favorite3Id"]),
                Favorite4Id = Convert.ToInt32(projectFile["Favorite4Id"])
            };

            // save object to json file
            Dictionary<string, string?> dictionary = newFile.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(newFile)?.ToString());
            jsonString = JsonConvert.SerializeObject(dictionary);
            File.WriteAllText(newFile.ProjectFilePath, jsonString);

            return newFile;
        }

        /// <summary>
        /// Method to provide a default project file object at application startup.
        /// </summary>
        /// <returns>Return the ProjectFile object.</returns>
        public static ProjectFile AtStartup()
        {
            ProjectFile file = new()
            {
                ProjectName = "Load project"
            };
            return file;
        }

        /// <summary>
        /// Method to generate a list of currently selected input file types. The formats are of type .jpg and .JPG. Always with leading dot and upper and lower case version of the extension.
        /// </summary>
        /// <returns>Returns a list of strings indicating the file formats.</returns>
        public List<string> GetInputFileTypes()
        {
            List<string> inputList = new();

            // add file types to list
            if (NefFilesChecked == true) { inputList.AddRange(ImageFile.NefStrings.ToList()); }
            if (OrfFilesChecked == true) { inputList.AddRange(ImageFile.OrfStrings.ToList()); }
            if (JpgFilesChecked == true) { inputList.AddRange(ImageFile.JpgStrings.ToList()); }
            if (PngFilesChecked == true) { inputList.AddRange(ImageFile.PngStrings.ToList()); }
            if (TifFilesChecked == true) { inputList.AddRange(ImageFile.TifStrings.ToList()); }

            return inputList;
        }

        /// <summary>
        /// Method to save changed made to te project file.
        /// </summary>
        public void Save()
        {
            // save object to json file
            Dictionary<string, string?> dictionary = GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(this)?.ToString());
            string jsonString = JsonConvert.SerializeObject(dictionary);
            File.WriteAllText(ProjectFilePath, jsonString);
        }
    }
}
