using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mako.Cloud;

public class Geocode
{
    private string _apiKey;
    
    public Geocode(string apiKey)
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
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(uri.Uri);
            var body = response.Content.ReadAsStringAsync();
        }

        return (0, 0);

    }
}
