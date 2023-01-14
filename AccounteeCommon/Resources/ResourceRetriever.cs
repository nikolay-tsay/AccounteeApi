using System.Globalization;

namespace AccounteeCommon.Resources;

public static class ResourceRetriever
{
    public static string Get(CultureInfo cultureInfo, string name) => Resources.ResourceManager.GetString(name, cultureInfo)!;

    public static string Get(CultureInfo cultureInfo, string name, params object[] parameters)
    {
        var localizedString = Resources.ResourceManager.GetString(name, cultureInfo)!;

        return string.Format(localizedString, parameters);
    }
}