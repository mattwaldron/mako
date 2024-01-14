namespace Mako.Cloud;

public static class IdentifiedExtensions
{
    public static IEnumerable<T> StripId<T>(this IEnumerable<Identified<T>> withId)
    {
        return withId.Select(v => v.Content);
    }
}
