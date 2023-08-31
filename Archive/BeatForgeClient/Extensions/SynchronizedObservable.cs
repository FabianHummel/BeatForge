using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BeatForgeClient.Extensions;

public sealed class SynchronizedObservable<T> : ObservableCollection<T>
{
    private readonly ICollection<T> _sourceCollection;

    public SynchronizedObservable(ICollection<T> sourceCollection) : base(sourceCollection)
    {
        this._sourceCollection = sourceCollection;
        CollectionChanged += SynchronizedObservable_CollectionChanged;
    }

    private void SynchronizedObservable_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (var p in e.NewItems?.Cast<T>() ?? Enumerable.Empty<T>())
        {
            _sourceCollection.Add(p);
        }
        foreach (var p in e.OldItems?.Cast<T>() ?? Enumerable.Empty<T>())
        {
            _sourceCollection.Remove(p);
        }
    }
}