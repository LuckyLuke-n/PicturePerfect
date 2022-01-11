using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    internal class SettingsFile
    {
        private List<Machine> MachineList = new();

        public SettingsFile()
        {
            
        }

        private class Machine
        {
            public string MachineName { get; set; }
            public string PathToProjectFile { get; set; }
            public string PathToProject { get; set; }

            public Machine(string name, string pathToFile, string pathToFolder)
            {

            }
        }
        public static SettingsFile Load()
        {
            string jsonString = File.ReadAllText(@"Assets/settings.json");
            SettingsFile file = JsonConvert.DeserializeObject<SettingsFile>(jsonString);
            if (jsonString != null && file != null)
            {
                if (file.MachineList.Contains(Environment.MachineName) == true)
                {
                    return file;
                }
                else
                {
                    return New();
                }
                return JsonConvert.DeserializeObject<SettingsFile>(jsonString);
            }
            else
            {
                // create a new file
                return New();
            }
        }

        public static SettingsFile New()
        {
            SettingsFile file = new();
            string jsonString = JsonConvert.SerializeObject(file);
            File.WriteAllText(@"Assets/settings.json", jsonString);
            return file;
        }

        public void Add(string pathToProjectFile, string pathToProjectFolder)
        {
            MachineList.Add(new Machine(name: Environment.MachineName, pathToFile: pathToProjectFile, pathToFolder: pathToProjectFolder));
            Save();
        }

        private void Save()
        {
            string jsonString = JsonConvert.SerializeObject(this);
            File.WriteAllText(@"Assets/settings.json", jsonString);
        }
    }
}
