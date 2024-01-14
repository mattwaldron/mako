using System.Text.Json;

namespace Mako.Common;

public static class Json
{
    public static readonly JsonSerializerOptions opts = new JsonSerializerOptions
    {
        AllowTrailingCommas = true,
        WriteIndented = true,
        IncludeFields = true
    };

    public static string ToJson<T>(this T obj)
    {
        return JsonSerializer.Serialize<T>(obj, opts);
    }

    public static void ToJsonFile<T>(this T obj, string file)
    {
        File.WriteAllText(file, obj.ToJson());
    }

    public static T? FromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, opts);
    }

    public static T? FromFile<T>(string file)
    {
        return Json.FromJson<T>(File.ReadAllText(file));
    }
}
