using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DalFacade
{
    public static class FileReader
    {
        //TODO: Needs automation
        public static string GetFilePath(string filename)
        {
            var txtFileLoc1 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName + $"\\{filename}";
            var txtFileLoc2 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + $"\\{filename}";
            var txtFileLoc3 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + $"\\{filename}";

            if (File.Exists(txtFileLoc1))
                return txtFileLoc1;
            else if (File.Exists(txtFileLoc2))
                return txtFileLoc2;

            return txtFileLoc3;

        }

        public static string GetFolderPath(string foldername)
        {
            var folderLoc1 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName + $"\\{foldername}";
            var folderLoc2 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + $"\\{foldername}";
            var folderLoc3 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + $"\\{foldername}";

            if (Directory.Exists(folderLoc1))
                return folderLoc1;
            else if (Directory.Exists(folderLoc2))
                return folderLoc2;

            return folderLoc3;
        }
        public static List<string> GetFileNames(string directory)
        {
            return 
                Directory.GetFiles(GetFolderPath(directory)).
                Select(file => Path.GetFileNameWithoutExtension(file))
                .ToList();
        }

        public static List<string> LoadJson(string filename)
        {
            if (filename.EndsWith(".json"))
                return JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(GetFilePath(filename)));

            throw new Exception($"{filename} is not json");
        }

        public static List<string> LoadTxt(string filename)
        {
            return File.ReadAllLines(GetFilePath(filename)).ToList();
        }
    }
}
