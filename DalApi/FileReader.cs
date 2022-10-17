using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DalFacade
{
    public static class FileReader
    {
        public static string GetFilePath(string filename, List<string> extensions)
        {
            var Loc1 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var Loc2 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            var Loc3 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            var plPath = "";

            if (Loc1.Contains("PL"))
                plPath = Loc1;
            else if (Loc2.Contains("PL"))
                plPath = Loc2;
            else plPath = Loc3;

            return Directory
                .GetFiles(plPath, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
                .First(f => f.Contains(filename));
        }

        public static string GetFolderPath(string foldername)
        {
            var folderLoc1 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName + $"\\{foldername}";
            var folderLoc2 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + $"\\{foldername}";
            var folderLoc3 = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + $"\\{foldername}";

            if (Directory.Exists(folderLoc1))
                return folderLoc1;

            return Directory.Exists(folderLoc2) ? folderLoc2 : folderLoc3;
        }
        public static List<string> GetFileNames(string directory, List<string> extensions)
        {
            return Directory.GetFiles(GetFolderPath(directory), "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public static List<string> LoadJson(string filename)
        {
            return JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(GetFilePath(filename, new List<string> { ".json" })));
        }

        public static List<string> LoadTxt(string filename)
        {
            return File.ReadAllLines(GetFilePath(filename, new List<string>{".txt"})).ToList();
        }
    }
}
