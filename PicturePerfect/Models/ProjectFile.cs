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
        public string ProjectFolder { get; private set; } = string.Empty;
        public string DatabasePath { get; private set; } = string.Empty;
        private string Release { get; set; } = string.Empty;
        #endregion

        #region Theme
        public string DarkColor { get; set; } = ThisApplication.DarkColorDefault;
        public string MediumColor { get; set; } = ThisApplication.MediumColorDefault;
        public string LightColor { get; set; } = ThisApplication.LightColorDefault;
        public string LightFontColor { get; set; } = ThisApplication.LightFontColorDefault;
        public string DarkContrastColor { get; set; } = ThisApplication.DarkContrastColorDefault;
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
            ProjectFile newFile = new ProjectFile()
            {
                Release = ThisApplication.ApplicationVersion,
                ProjectName = file.ProjectName,
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
    }
}
