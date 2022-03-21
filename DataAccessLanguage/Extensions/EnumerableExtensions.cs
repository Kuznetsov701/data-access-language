﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<object> Select(this IEnumerable<object> list, int startIndex, int endIndex)
        {
            if (startIndex > endIndex)
                throw new ArgumentException("endIndex less then startIndex");

            if (startIndex < 0 || startIndex > list.Count() - 1)
                throw new ArgumentOutOfRangeException("startIndex out of range");

            if (endIndex < 0 || endIndex > list.Count() - 1)
                throw new ArgumentOutOfRangeException("endIndex out of range");

            List<object> res = new List<object>();
            for (int i = startIndex; i <= endIndex; i++)
                res.Add(list.ElementAt(i));

            return res;
        }

        public static IEnumerable<object> Select(this IDictionary list, Func<object, object> func)
        {
            foreach (var item in list)
                yield return func(item);
        }

        public static async Task<IEnumerable<K>> ParallelSelectAsync<T, K>(this IEnumerable<T> list, Func<T, Task<K>> func)
        {
            IEnumerable<Task<K>> tasks = list.Select(x => func(x)).ToArray();
            await Task.WhenAll(tasks);
            return tasks.Select(x => x.Result).ToList();
        }

        public static async Task<IEnumerable<object>> SelectAsync(this IDictionary list, Func<object, Task<object>> func)
        {
            List<object> res = new List<object>();
            foreach(var item in list)
                res.Add(await func(item));
            return res;
        }

        public static async Task<IEnumerable<K>> SelectAsync<T, K>(this IEnumerable<T> list, Func<T, Task<K>> func)
        {
            List<K> result = new List<K>();
            foreach (var i in list)
                result.Add(await func(i));
            return result;
        }

        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var i in list)
                action(i);
            return list;
        }

        public static bool SetValues(this IList<object> list, int startIndex, int endIndex, object value)
        {
            if (startIndex > endIndex)
                throw new ArgumentException("endIndex less then startIndex");

            if (startIndex < 0 || startIndex > list.Count - 1)
                throw new ArgumentOutOfRangeException("startIndex out of range");

            if (endIndex < 0 || endIndex > list.Count - 1)
                throw new ArgumentOutOfRangeException("endIndex out of range");

            for (int i = startIndex; i <= endIndex; i++)
                list[i] = value;

            return true;
        }

        public static bool TrySetValue(this IList<object> list, int index, object value)
        {
            if (list.Count > index)
                list[index] = value;
            else if (list.Count == index)
                list.Add(value);
            else
                return false;
            return true;
        }
    }
}