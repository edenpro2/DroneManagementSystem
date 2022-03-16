using System.Runtime.Serialization;

namespace DO
{
    [DataContract]
    public enum DroneStatuses
    {
        [EnumMember(Value = "0")]
        Free,
        [EnumMember(Value = "1")]
        Maintenance,
        [EnumMember(Value = "2")]
        Delivery
    }

    [DataContract]
    public enum WeightCategories : short
    {
        [EnumMember(Value = "0")]
        Light,
        [EnumMember(Value = "1")]
        Medium,
        [EnumMember(Value = "2")]
        Heavy
    }

    [DataContract]
    public enum Priorities : short
    {
        [EnumMember(Value = "0")]
        Regular,
        [EnumMember(Value = "1")]
        Fast,
        [EnumMember(Value = "2")]
        Emergency
    }

    [DataContract]
    public enum Maximum : short
    {
        Stations = 5,
        Customers = 100,
        Packages = 1000
    }

    // in km/h
    [DataContract]
    public enum Speeds
    {
        Unloaded = 12,
        Loaded = 8
    }
}