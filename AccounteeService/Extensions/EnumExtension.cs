using Newtonsoft.Json;

namespace AccounteeService.Extensions;

public static class EnumExtension
{
    public static string ToEnumString<T>(this T value) where T : Enum 
        => JsonConvert.SerializeObject(value).Replace("\"", "");
    

    public static T? ToEnum<T>(this string value) where T : Enum 
        => JsonConvert.DeserializeObject<T>($"\"{value}\"");
    
}