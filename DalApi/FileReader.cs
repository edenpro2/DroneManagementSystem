#nullable enable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using DalFacade.DO;
using System.Data.SqlTypes;

namespace DalFacade
{
    public static class FileReader
    {
        public static string GetFilePath(string filename, List<string> extensions)
        {
            var projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            var plPath = projectDirectory + "\\PL";

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
            var projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

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

        public class NominatimJson
        {
            public int place_id { get; set; }
            public string licence { get; set; }
            public string osm_type { get; set; }
            public int osm_id { get; set; }
            public List<string> boundingbox { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
            public int place_rank { get; set; }
            public string category { get; set; }
            public string type { get; set; }
            public double importance { get; set; }
        }



        public static NominatimJson LoadNominatim(Location Loc)
        {
            var url = $"https://nominatim.openstreetmap.org/search.php?q={Loc.Latitude}%2C{Loc.Longitude}&format=jsonv2";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "My C# Project -> thank you for letting us use your api :)";
            request.Method = "GET";
            var myResponse = (HttpWebResponse)request.GetResponse();
            Stream newStream = myResponse.GetResponseStream();
            StreamReader sr = new StreamReader(newStream);
            var result = sr.ReadToEnd();

            var res = result.Remove(0, 1);
            res = res.Remove(res.Length - 1, 1);


            return JsonConvert.DeserializeObject<NominatimJson>(res);
        }

        public static IList<string> LoadJson(string filename)
        {
            var dir = GetFilePath(filename, new List<string> { ".json" });

            if (dir == null)
                throw new NullReferenceException("The directory was null - filename might be wrong");

            return (JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(dir)) ?? throw new InvalidOperationException()).ToList();
        }

        public static IEnumerable<string> LoadTxt(string filename)
        {
            return File.ReadAllLines(GetFilePath(filename, new List<string> { ".txt" })).ToList();
        }
    }
}
