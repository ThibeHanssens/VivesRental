using Vives.Services.Model;

namespace Vives.Services.Extensions;

public static class SortingExtensions
{
    public static string ToOrderQueryString(this Sorting sorting)
    {
        if (string.IsNullOrWhiteSpace(sorting.PropertyName))
        {
            return string.Empty;
        }

        //Map to sortString
        var order = sorting.IsDescending ? "desc" : "";
        return $"{sorting.PropertyName} {order}".Trim();
    }
}