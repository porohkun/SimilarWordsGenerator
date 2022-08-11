using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace System.Collections.ObjectModel
{
    public class WrappedObservableCollection<TValue, TSource> : ObservableCollection<TValue>, ICollection<TValue>, IList
    {
        private ObservableCollection<TSource> _source;
        private Func<TSource, TValue> _wrap;
        private Func<TValue, TSource, bool> _compare;
        public WrappedObservableCollection(ObservableCollection<TSource> source, Func<TSource, TValue> wrap, Func<TValue, TSource, bool> compare)
        {
            _source = source;
            _wrap = wrap;
            _compare = compare;

            foreach (var item in _source)
                base.Add(_wrap.Invoke(item));
            _source.CollectionChanged += SourceCollectionChanged;
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                        base.Add(_wrap.Invoke((TSource)item));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var forRemove = new List<TValue>();
                    foreach (var item in e.OldItems)
                        forRemove.Add(this.First(entry => _compare(entry, (TSource)item)));
                    foreach (var item in forRemove)
                        base.Remove(item);
                    break;
                default: throw new NotImplementedException();
            }
        }

        private object NotSupported() => throw new NotSupportedException("This collection is wrapping collection and can't be modified directrly. You need to modify source collection.");

        //
        // Summary:
        //     Gets a value indicating whether the System.Collections.Generic.ICollection`1
        //     is read-only.
        //
        // Returns:
        //     true if the System.Collections.Generic.ICollection`1 is read-only; otherwise,
        //     false. In the default implementation of System.Collections.ObjectModel.Collection`1,
        //     this property always returns false.
        bool ICollection<TValue>.IsReadOnly => true;

        //
        // Summary:
        //     Gets or sets the element at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to get or set.
        //
        // Returns:
        //     The element at the specified index.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.IList.
        //
        //   T:System.ArgumentException:
        //     The property is set and value is of a type that is not assignable to the System.Collections.IList.
        object IList.this[int index]
        {
            get => base[index];
            set => NotSupported();
        }

        //
        // Summary:
        //     Gets a value indicating whether the System.Collections.IList is read-only.
        //
        // Returns:
        //     true if the System.Collections.IList is read-only; otherwise, false. In the default
        //     implementation of System.Collections.ObjectModel.Collection`1, this property
        //     always returns false.
        bool IList.IsReadOnly => true;

        //
        // Summary:
        //     Adds an object to the end of the System.Collections.ObjectModel.Collection`1.
        //
        // Parameters:
        //   item:
        //     The object to be added to the end of the System.Collections.ObjectModel.Collection`1.
        //     The value can be null for reference types.
        new public void Add(TValue item) => NotSupported();

        //
        // Summary:
        //     Removes all elements from the System.Collections.ObjectModel.Collection`1.
        new public void Clear() => NotSupported();

        //
        // Summary:
        //     Inserts an element into the System.Collections.ObjectModel.Collection`1 at the
        //     specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert. The value can be null for reference types.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than zero. -or- index is greater than System.Collections.ObjectModel.Collection`1.Count.
        new public void Insert(int index, TValue item) => NotSupported();

        //
        // Summary:
        //     Removes the first occurrence of a specific object from the System.Collections.ObjectModel.Collection`1.
        //
        // Parameters:
        //   item:
        //     The object to remove from the System.Collections.ObjectModel.Collection`1. The
        //     value can be null for reference types.
        //
        // Returns:
        //     true if item is successfully removed; otherwise, false. This method also returns
        //     false if item was not found in the original System.Collections.ObjectModel.Collection`1.
        new public bool Remove(TValue item) => (bool)NotSupported();

        //
        // Summary:
        //     Removes the element at the specified index of the System.Collections.ObjectModel.Collection`1.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to remove.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than zero. -or- index is equal to or greater than System.Collections.ObjectModel.Collection`1.Count.
        new public void RemoveAt(int index) => NotSupported();

        //
        // Summary:
        //     Adds an item to the System.Collections.IList.
        //
        // Parameters:
        //   value:
        //     The System.Object to add to the System.Collections.IList.
        //
        // Returns:
        //     The position into which the new element was inserted.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     value is of a type that is not assignable to the System.Collections.IList.
        int IList.Add(object value) => (int)NotSupported();

        //
        // Summary:
        //     Inserts an item into the System.Collections.IList at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which value should be inserted.
        //
        //   value:
        //     The System.Object to insert into the System.Collections.IList.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.IList.
        //
        //   T:System.ArgumentException:
        //     value is of a type that is not assignable to the System.Collections.IList.
        void IList.Insert(int index, object value) => NotSupported();

        //
        // Summary:
        //     Removes the first occurrence of a specific object from the System.Collections.IList.
        //
        // Parameters:
        //   value:
        //     The System.Object to remove from the System.Collections.IList.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     value is of a type that is not assignable to the System.Collections.IList.
        void IList.Remove(object value) => NotSupported();


    }
}
