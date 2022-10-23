#nullable enable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

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


            return
                Directory
                    .GetFiles(plPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
                    .First(f => f.Contains(Path.GetFileNameWithoutExtension(filename)));
        }

        public enum PathOption
        {
            CreateDirectory,
            SearchOnly
        }

        public static string GetFolderPath(string folder, PathOption pathOption = PathOption.SearchOnly, SearchOption searchOption = SearchOption.AllDirectories)
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;

            //Todo: Fix
            if (projectDirectory!.Contains(folder))
                return projectDirectory;

            var dir =
                Directory.GetDirectories(projectDirectory, "*", searchOption)
                    .FirstOrDefault(sub => sub.EndsWith(folder));

            if (dir == null)
            {
                dir = projectDirectory + $"\\{folder}";
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static IList<string?> GetFileNames(string directory, List<string> extensions, SearchOption option)
        {
            var dir = GetFolderPath(directory);

            return
                Directory.GetFiles(dir, "*.*", option)
                    .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();
        }

        public static IList<string> LoadJson(string filename)
        {
            var dir = GetFilePath(filename, new List<string> {".json"});

            if (dir == null)
                throw new NullReferenceException("The directory was null - filename might be wrong");

            return JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(dir)).ToList();
        }

        public static IEnumerable<string> LoadTxt(string filename)
        {
            return File.ReadAllLines(GetFilePath(filename, new List<string> {".txt"})).ToList();
        }
    }
}
