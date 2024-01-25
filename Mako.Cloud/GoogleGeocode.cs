using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using Mako.Common;
using MongoDB.Driver;

namespace Mako.Cloud;


public class GoogleGeocode
{
    private string _apiKey;
    
    public GoogleGeocode(string apiKey)
    {
        _apiKey = apiKey;
    }
    
    public async Task<(double latitude, double longitude)> Coordinates (string address)
    {
        var uri = new UriBuilder("https://maps.googleapis.com/maps/api/geocode/json");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["key"] = _apiKey;
        query["address"] = address;
        uri.Query = query.ToString();
        using var client = new HttpClient();
        
        var response = await client.GetAsync(uri.Uri);
        var body = response.Content.ReadAsStringAsync().Result;
        var parsed = Json.FromString<GoogleForwardGeocodeResponse>(body);
        if (parsed.status != "OK")
        {
            throw new Exception($"Location lookup failed for {address}");
        }
        var latlong = parsed.results.First().geometry.location;
        return (latlong.lat, latlong.lng);
    }

    public async Task<int> CoordinatesToTimeOffset(double latitude, double longitude)
    {
        var uri = new UriBuilder("https://maps.googleapis.com/maps/api/timezone/json");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["key"] = _apiKey;
        query["location"] = $"{latitude},{longitude}";
        query["timestamp"] = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}";
        uri.Query = query.ToString();
        using var client = new HttpClient();

        var response = await client.GetAsync(uri.Uri);
        var body = response.Content.ReadAsStringAsync().Result;
        var parsed = Json.FromString<GoogleTimeZoneResponse>(body);
        if (parsed.status != "OK")
        {
            throw new Exception($"Timezone lookup failed for {latitude}, {longitude}");
        }
        return parsed.rawOffset + parsed.dstOffset;
    }

    public async Task<string> CoordinatesToTimeZoneName(double latitude, double longitude)
    {
        var uri = new UriBuilder("https://maps.googleapis.com/maps/api/timezone/json");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["key"] = _apiKey;
        query["location"] = $"{latitude},{longitude}";
        query["timestamp"] = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}";
        uri.Query = query.ToString();
        using var client = new HttpClient();

        var response = await client.GetAsync(uri.Uri);
        var body = response.Content.ReadAsStringAsync().Result;
        var parsed = Json.FromString<GoogleTimeZoneResponse>(body);
        if (parsed.status != "OK")
        {
            throw new Exception($"Timezone lookup failed for {latitude}, {longitude}");
        }
        return parsed.timeZoneName;
    }
}

internal record GoogleForwardGeocodeResponse
{
    public record Result
    {
        public record AddressComponent
        {
            public string long_name;
            public string short_name;
            public string[] types;
        }

        public AddressComponent[] address_components;
        public string formatted_address;

        public record Geometry
        {
            public record Location
            {
                public double lat;
                public double lng;
            }

            public Location location;
            public string location_type;
            public Dictionary<string, Location> viewport;
        }

        public Geometry geometry;
        public string place_id;

        public record PlusCode
        {
            public string compound_code;
            public string global_code;
        }

        public PlusCode plus_code;

        public string[] types;
    }
    public Result[] results;

    public string status;
}

internal record GoogleTimeZoneResponse
{
    public int dstOffset;
    public int rawOffset;
    public string timeZoneId;
    public string timeZoneName;

    public string status;
}
