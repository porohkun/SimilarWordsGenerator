using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> map, IDictionary<TKey, TValue> mergingDictionary) => Merge(map, mergingDictionary, TakeLeft);
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> map, IDictionary<TKey, TValue> mergingDictionary, bool keepExisting = true)
        {
            if (keepExisting)
                Merge(map, mergingDictionary, TakeLeft);
            else
                Merge(map, mergingDictionary, TakeRight);
        }
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> map, IDictionary<TKey, TValue> mergingDictionary, Func<TValue, TValue, TValue> selector = null)
        {
            foreach (var pair in mergingDictionary)
            {
                if (map.TryGetValue(pair.Key, out TValue oldValue))
                    map[pair.Key] = selector(oldValue, pair.Value);
                else
                    map[pair.Key] = pair.Value;
            }
        }

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> map, IReadOnlyDictionary<TKey, TValue> mergingDictionary) => Merge(map, mergingDictionary, TakeLeft);
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> map, IReadOnlyDictionary<TKey, TValue> mergingDictionary, bool keepExisting = true)
        {
            if (keepExisting)
                Merge(map, mergingDictionary, TakeLeft);
            else
                Merge(map, mergingDictionary, TakeRight);
        }
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> map, IReadOnlyDictionary<TKey, TValue> mergingDictionary, Func<TValue, TValue, TValue> selector = null)
        {
            foreach (var pair in mergingDictionary)
            {
                if (map.TryGetValue(pair.Key, out TValue oldValue))
                    map[pair.Key] = selector(oldValue, pair.Value);
                else
                    map[pair.Key] = pair.Value;
            }
        }

        private static T TakeLeft<T>(T left, T right) => left;
        private static T TakeRight<T>(T left, T right) => right;
        public static IEnumerable<TResult> Glue<T1, T2, TResult>(this IEnumerable<T1> collection, IEnumerable<T2> other, Func<T1, T2, TResult> selector)
        {
            var enumerator1 = collection.GetEnumerator();
            var enumerator2 = other.GetEnumerator();

            var next1 = enumerator1.MoveNext();
            var next2 = enumerator2.MoveNext();

            while (next1 && next2)
            {
                yield return selector.Invoke(enumerator1.Current, enumerator2.Current);
                next1 = enumerator1.MoveNext();
                next2 = enumerator2.MoveNext();
            }

            if (!next1)
                while (next2)
                {
                    yield return selector.Invoke(default(T1), enumerator2.Current);
                    next2 = enumerator2.MoveNext();
                }

            if (!next2)
                while (next1)
                {
                    yield return selector.Invoke(enumerator1.Current, default(T2));
                    next1 = enumerator2.MoveNext();
                }
        }

        public static void Clear<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = default(T);
        }
    }
}
