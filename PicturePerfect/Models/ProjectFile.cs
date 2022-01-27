using Newtonsoft.Json;
using PicturePerfect.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// Get the notes for this project.
        /// </summary>
        public string Notes { get; private set; } = string.Empty;
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
            set { favorite1Id = value; }
        }
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite2Id
        {
            get { return favorite2Id; }
            set { favorite2Id = value;}
        }
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite3Id
        {
            get { return favorite3Id; }
            set { favorite3Id = value;  }
        }
        /// <summary>
        /// Get or set the image id. Set id to 0 if no favorite is selected.
        /// </summary>
        public int Favorite4Id
        {
            get { return favorite4Id; }
            set { favorite4Id = value; }
        }
        #endregion

        #region Settings
        private int bufferSize = 5;
        /// <summary>
        /// Get or set the buffer size for converting images while viewing.
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }
        private List<string> inputFormats = new() { RawTypes.orf.ToString(), RawTypes.raw.ToString() };
        /// <summary>
        /// Get the input formats. To set use the public method SetInputFormats().
        /// </summary>
        public List<string> InputFormats
        {
            get { return inputFormats; }
            private set { inputFormats = value; }
        }

        private bool useSeparator = false;
        /// <summary>
        /// Get or set the use separator property.
        /// </summary>
        public bool UseSeparator
        {
            get { return useSeparator; }
            set { useSeparator = value; }
        }

        private string? separator = null;
        /// <summary>
        /// Get or set the seprator character for folder naming.
        /// </summary>
        public string? Separator
        {
            get { return separator; }
            set { separator = value; }
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
                DatabasePath = Path.Combine(path, "sqlite", "database.sqlite")
            };

            // create basic folders
            Directory.CreateDirectory(new FileInfo(file.DatabasePath).DirectoryName);
            Directory.CreateDirectory(file.ImageFolder);

            // save object to json file
            string jsonString = JsonConvert.SerializeObject(file);
            File.WriteAllText(file.ProjectFilePath, jsonString);

            return file;
        }

        /// <summary>
        /// Method to load the project file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the project file object.</returns>
        public static ProjectFile Load(string path)
        {
            // load current file
            string jsonString = File.ReadAllText(path);
            ProjectFile file = JsonConvert.DeserializeObject<ProjectFile>(jsonString);

            List<string> inputList;

            // create new file object and carry over the information
            // this avoids compatibility issues in case the properties of this class are changed
            // update project file
            ProjectFile newFile = new()
            {
                Release = ThisApplication.ApplicationVersion,
                ProjectName = file.ProjectName,
                ProjectFilePath = path,
                ProjectOwner = file.ProjectOwner,
                CreationDate = file.CreationDate,
                Notes = file.Notes,
                ImageFolder = Path.Combine(new FileInfo(path).DirectoryName, "images"),
                DatabasePath = Path.Combine(new FileInfo(path).DirectoryName, "sqlite", "database.sqlite"),
                // settings
                InputFormats = file.InputFormats.Distinct().ToList(), // fix for duplicating bug. this is no good solution
                BufferSize = file.BufferSize,
                Separator = file.Separator
            };

            // save object to json file
            jsonString = JsonConvert.SerializeObject(newFile);
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
        /// Method to set the input file types.
        /// </summary>
        /// <param name="rawInputs"></param>
        /// <param name="otherInputs"></param>
        public void SetInputFormats(List<RawTypes> rawInputs, List<ImageTypes>? otherInputs = null)
        {
            List<string>  fileTypes = new List<string>();
            
            // insert raw types in list
            foreach (RawTypes rawInput in rawInputs)
            {
                fileTypes.Add(rawInput.ToString());
            }

            // insert oter inputs in list
            if (otherInputs != null)
            {
                foreach (ImageTypes otherInput in otherInputs)
                {
                    fileTypes.Add(otherInput.ToString());
                }
            }

            // set property
            InputFormats = fileTypes;

            Save();
        }

        /// <summary>
        /// Method to save changed made to te project file.
        /// </summary>
        public void Save()
        {
            // save to json file
            string jsonString = JsonConvert.SerializeObject(this);
            File.WriteAllText(ProjectFilePath, jsonString);
        }
    }
}
