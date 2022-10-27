using System.Collections.Generic;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace BL.BO.OSM
{
    public class NominatimJson
    {
        public int place_id { get; set; } = -1;
        public string licence { get; set; } = string.Empty;
        public string osm_type { get; set; } = string.Empty;
        public int osm_id { get; set; } = -1;
        public List<string> boundingbox { get; set; } = new();
        public string lat { get; set; } = string.Empty;
        public string lon { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int place_rank { get; set; } = -1;
        public string category { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public double importance { get; set; } = -1.0;
    }
}
