using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BeatForgeClient.Extensions;

public static class CollectionExtensions
{
    public static void AddRange<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
    {
        foreach (var item in items)
        {
            source.Add(item);
        }
    }

    public static void ReplaceAll<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
    {
        source.Clear();
        AddRange(source, items);
    }
}