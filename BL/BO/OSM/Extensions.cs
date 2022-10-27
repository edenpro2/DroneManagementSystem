using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Location = DalFacade.DO.Location;

namespace BL.BO.OSM
{
    public static class Extensions
    {
        private static NominatimJson LocationMetaData(Location location)
        {
            var myResponse = GetResponse(location).Result;

            var responseStream = myResponse.GetResponseStream();
            if (responseStream == null)
                return new NominatimJson();

            var readerOutput = new StreamReader(responseStream).ReadToEnd();

            readerOutput = readerOutput.Remove(0, 1);
            readerOutput = readerOutput.Remove(readerOutput.Length - 1, 1);

            // Remove used twice to adjust data to fit json deserializer
            return JsonConvert.DeserializeObject<NominatimJson>(readerOutput);
        }

        public static string LocationAddress(Location location) => LocationMetaData(location).display_name;

        private static async Task<HttpWebResponse> GetResponse(Location location)
        {
            var request = (HttpWebRequest)WebRequest
                .Create($"https://nominatim.openstreetmap.org/search.php?q={location.Latitude}%2C{location.Longitude}&format=jsonv2");
            request.UserAgent = "My C# Project -> thank you for letting us use your api :)";
            request.Method = "GET";

            return (HttpWebResponse)
                await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
        }
    }
}