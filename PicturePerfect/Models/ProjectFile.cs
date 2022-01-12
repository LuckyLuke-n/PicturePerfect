using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    internal class ProjectFile
    {
        #region General
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectOwner { get; private set; } = string.Empty;
        public DateTime CreationDate { get; private set; } = DateTime.Now;
        public string Notes { get; private set; } = string.Empty;
        public string ProjectFilePath { get; private set; } = string.Empty;
        public string ProjectFolder { get; private set; } = string.Empty;
        public string DatabasePath { get; private set; } = string.Empty;
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
            set { darkColor = value; Save(); }
        }
        /// <summary>
        /// Get or set the value for the medium theme color.
        /// </summary>
        public string MediumColor
        {
            get { return mediumColor; }
            set { mediumColor = value; Save(); }
        }
        /// <summary>
        /// Get or set the value for the light theme color.
        /// </summary>
        public string LightColor
        {
            get { return lightColor; }
            set { lightColor = value; Save(); }
        }
        /// <summary>
        /// Get or set the value for the light font theme color.
        /// </summary>
        public string LightFontColor
        {
            get { return lightFontColor; }
            set { lightFontColor = value; Save(); }
        }
        /// <summary>
        /// Get or set the value for the dark contrast theme color.
        /// </summary>
        public string DarkContrastColor
        {
            get { return darkContrastColor; }
            set { darkContrastColor = value; Save(); }
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
        /// Method to create a new project file. The file is saved to the selected folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns>Return the project file.</returns>
        public static ProjectFile New(string path, string name)
        {
            // new project file object
            ProjectFile file = new()
            {
                Release = ThisApplication.ApplicationVersion,
                ProjectName = name,
                ProjectFilePath = path,
                ProjectOwner = Environment.UserName,
                CreationDate = DateTime.Now,
                ProjectFolder = new FileInfo(path).Directory.FullName,
                DatabasePath = Path.Combine(new FileInfo(path).Directory.FullName, "database.sqlite")
            };

            // save to json file
            string jsonString = JsonConvert.SerializeObject(file);
            File.WriteAllText(path, jsonString);

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

            // create new file object and carry over the information
            // this avoids compatibility issues in case the properties of this class are changed
            // update project file
            ProjectFile newFile = new()
            {
                Release = ThisApplication.ApplicationVersion,
                ProjectName = file.ProjectName,
                ProjectFilePath = file.ProjectFilePath,
                ProjectOwner = file.ProjectOwner,
                CreationDate = file.CreationDate,
                Notes = file.Notes,
                ProjectFolder = file.ProjectFolder,
                DatabasePath = file.DatabasePath
                // add new properties and set the values!
            };

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
        /// Method to save changed made to te project file.
        /// </summary>
        private void Save()
        {
            // save to json file
            string jsonString = JsonConvert.SerializeObject(this);
            File.WriteAllText(ProjectFilePath, jsonString);
        }
    }
}
