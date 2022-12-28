﻿#nullable enable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DalFacade
{
    public static class FileReader
    {
        private static readonly string ProjectDirectory;

        public enum PathOption
        {
            CreateDirectory,
            SearchOnly
        }

        static FileReader()
        {
            var baseDir = @"..\..";
            string projectDirectory;

            do
            {
                projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseDir));
                baseDir += @"\..";
            } while (!projectDirectory.EndsWith("DroneManagementSystem"));

            ProjectDirectory = projectDirectory;
        }

        public static string GetFilePath(string filename, List<string> extensions)
        {
            var plPath = ProjectDirectory + "\\PL";

            return
                Directory
                    .GetFiles(plPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
                    .First(f => f.Contains(Path.GetFileNameWithoutExtension(filename)));
        }

        /// <summary>
        /// Return full path of folder (folder parameter must start without \\)
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="pathOption"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static string GetFolderPath(string folder, PathOption pathOption = PathOption.SearchOnly, SearchOption searchOption = SearchOption.AllDirectories)
        {
            var dir = Directory
                    .GetDirectories(ProjectDirectory, "*", searchOption)
                    .FirstOrDefault(sub => sub.EndsWith(folder));

            if (dir == null)
            {
                dir = ProjectDirectory + $"\\{folder}";
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static IList<string?> GetFileNames(string directory, List<string> extensions, SearchOption option)
        {
            var dir = GetFolderPath(directory);

            return Directory
                .GetFiles(dir, "*.*", option)
                .Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public static List<T> LoadJson<T>(string filename)
        {
            var dir = GetFilePath(filename, new List<string> { ".json" });

            if (dir == null)
                throw new NullReferenceException("The directory was null - filename might be wrong");

            return (JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(dir)) ?? throw new InvalidOperationException()).ToList();
        }

        public static IEnumerable<string> LoadTxt(string filename)
        {
            return File.ReadAllLines(GetFilePath(filename, new List<string> { ".txt" })).ToList();
        }
    }
}
