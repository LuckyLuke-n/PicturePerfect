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
        public string ProjectOwner { get; private set; }
        public DateTime CreationDate { get; private set; }
        public string Notes { get; private set; } = string.Empty;

        public ProjectFile()
        {

        }

        /// <summary>
        /// Method to create a new project file. The file is saved to the selected folder.
        /// </summary>
        /// <returns>Return the project file.</returns>
        public static ProjectFile New(string path)
        {
            //UserSettings.Default.PathToProjectFile = path;
            ProjectFile file = new();
            file.ProjectOwner = Environment.UserName;
            file.CreationDate = DateTime.Now;

            //string jsonString = JsonConvert.SerializeObject(file);
            //File.WriteAllText(UserSettings.Default.PathToProjectFile, jsonString);

            return file;
        }

        /// <summary>
        /// Method to load the project file.
        /// </summary>
        /// <returns>Returns the project file object.</returns>
        public static ProjectFile Load()
        {
            //string jsonString = File.ReadAllText(UserSettings.Default.PathToProjectFile);
            //ProjectFile file = JsonConvert.DeserializeObject<ProjectFile>(jsonString);
            //return file;

            return new ProjectFile();
        }
    }
}
