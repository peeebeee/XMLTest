// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var anprStuff = AnprStuff.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class AnprStuff
    {
        [JsonProperty("Root")]
        public Root Root { get; set; }
    }

    public partial class Root
    {
        [JsonProperty("ANPREvents")]
        public AnprEvents AnprEvents { get; set; }
    }

    public partial class AnprEvents
    {
        [JsonProperty("ANPR")]
        public AnprEventsAnpr[] Anpr { get; set; }
    }

    public partial class AnprEventsAnpr
    {
        [JsonProperty("SiteName")]
        public string SiteName { get; set; }

        [JsonProperty("EquipmentUsed")]
        public string EquipmentUsed { get; set; }

        [JsonProperty("SiteId")]
        public SiteId SiteId { get; set; }

        [JsonProperty("SiteLocations")]
        public SiteLocations SiteLocations { get; set; }
    }

    public partial class SiteId
    {
        [JsonProperty("-i:nil")]
        public string INil { get; set; }
    }

    public partial class SiteLocations
    {
        [JsonProperty("ANPR")]
        public SiteLocationsAnpr[] Anpr { get; set; }
    }

    public partial class SiteLocationsAnpr
    {
        [JsonProperty("LocationForDisplay")]
        public LocationForDisplay LocationForDisplay { get; set; }
    }

    public partial class LocationForDisplay
    {
        [JsonProperty("Easting")]
        public Ing Easting { get; set; }

        [JsonProperty("Northing")]
        public Ing Northing { get; set; }

        [JsonProperty("Latitude")]
        public string Latitude { get; set; }

        [JsonProperty("Longitude")]
        public string Longitude { get; set; }
    }

    public enum Ing { The0 };

    public partial class AnprStuff
    {
        public static AnprStuff FromJson(string json) => JsonConvert.DeserializeObject<AnprStuff>(json, QuickType.Converter.Settings);
    }

    static class IngExtensions
    {
        public static Ing? ValueForString(string str)
        {
            switch (str)
            {
                case "0": return Ing.The0;
                default: return null;
            }
        }

        public static Ing ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Ing value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case Ing.The0: serializer.Serialize(writer, "0"); break;
            }
        }
    }

    public static class Serialize
    {
        public static string ToJson(this AnprStuff self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal class Converter: JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Ing) || t == typeof(Ing?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (t == typeof(Ing))
                return IngExtensions.ReadJson(reader, serializer);
            if (t == typeof(Ing?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return IngExtensions.ReadJson(reader, serializer);
            }
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = value.GetType();
            if (t == typeof(Ing))
            {
                ((Ing)value).WriteJson(writer, serializer);
                return;
            }
            throw new Exception("Unknown type");
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { 
                new Converter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
