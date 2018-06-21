using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

// ReSharper disable CheckNamespace

namespace NbPilot.Common
{
    public class HashDictionary : IDictionary<string, object>
    {
        private readonly Dictionary<string, string> _hashValueDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, object> _innerDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((IDictionary<string, string>)_hashValueDictionary).Add(new KeyValuePair<string, string>(item.Key, item.Value.CreateObjectHash()));
            ((IDictionary<string, object>)_innerDictionary).Add(item);
        }

        public void Clear()
        {
            _hashValueDictionary.Clear();
            _innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_innerDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<string, object>)_innerDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            _hashValueDictionary.Remove(item.Key);
            return ((IDictionary<string, object>)_innerDictionary).Remove(item);
        }

        public int Count
        {
            get { return _innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<string, object>)_innerDictionary).IsReadOnly; }
        }

        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }


        public void Add(string key, object value)
        {
            _hashValueDictionary.Add(key, value.CreateObjectHash());
            _innerDictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            _hashValueDictionary.Remove(key);
            return _innerDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public object this[string key]
        {
            get
            {
                object value;
                _innerDictionary.TryGetValue(key, out value);
                return value;
            }
            set
            {
                _hashValueDictionary[key] = value.CreateObjectHash();
                _innerDictionary[key] = value;
            }
        }
        public ICollection<string> Keys
        {
            get { return _innerDictionary.Keys; }
        }
        public ICollection<object> Values
        {
            get { return _innerDictionary.Values; }
        }

        #region hash
        
        public IDictionary<string, string> GetHashValues()
        {
            return _hashValueDictionary;
        }

        public string GetHashValue(string key)
        {
            var hashValues = GetHashValues();
            if (!hashValues.ContainsKey(key))
            {
                return string.Empty;
            }
            return _hashValueDictionary[key];
        }

        public T GetValueAs<T>(string key)
        {
            if (!this.ContainsKey(key))
            {
                return default(T);
            }
            return (T)this[key];
        }

        #endregion
    }
    
    #region extensions

    /// <summary>
    /// DictionaryWithHash Extensions
    /// </summary>
    public static class HashModelExtensions
    {
        /// <summary>
        /// 检测是否某一项改变
        /// </summary>
        /// <param name="hashModel"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckChanged(this HashDictionary hashModel, string key)
        {
            if (hashModel == null || key == null || !hashModel.ContainsKey(key))
            {
                return false;
            }
            var hashValue = hashModel.GetHashValue(key);
            return !hashModel[key].VerifyObjectHash(hashValue);
        }

        /// <summary>
        /// 检测是否有任何项改变
        /// </summary>
        /// <param name="hashModel"></param>
        /// <returns></returns>

        public static bool CheckAnyChanged(this HashDictionary hashModel)
        {
            if (hashModel == null)
            {
                return false;
            }

            var hashValues = hashModel.GetHashValues();
            foreach (var hashValue in hashValues)
            {
                var changedForKey = CheckChanged(hashModel, hashValue.Key);
                if (changedForKey)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 不能为空
        /// </summary>
        /// <param name="hashModel"></param>
        public static void ShouldNotNull(this HashDictionary hashModel)
        {
            if (hashModel == null)
            {
                throw new ArgumentNullException("hashModel");
            }
        }

        /// <summary>
        /// 按照指示决定是否添加值到属性中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashModel"></param>
        /// <param name="key"></param>
        /// <param name="shouldInclude"></param>
        /// <param name="getFunc"></param>
        /// <returns></returns>
        public static HashDictionary AutoAddOrUpdate<T>(this HashDictionary hashModel, string key, Func<string, bool> shouldInclude, Func<T> getFunc)
        {
            if (shouldInclude(key))
            {
                var value = getFunc();
                hashModel.Add(key, value);
            }
            return hashModel;
        }

        /// <summary>
        /// 按照是否包含并且变化决定调用处理程序（如果模型中确实包含某属性，并且确实检测到变化，才调用处理程序）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashModel"></param>
        /// <param name="key"></param>
        /// <param name="proecessAction"></param>
        public static void AutoProcessProperty<T>(this HashDictionary hashModel, string key, Action<T> proecessAction)
        {
            var value = hashModel.GetValueAs<T>(key);
            if (value != null && hashModel.CheckChanged(key))
            {
                proecessAction(value);
            }
        }

        /// <summary>
        /// 按照是否包含并且变化决定调用处理程序（如果模型中确实包含某属性，并且确实检测到变化，才调用处理程序）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hashModel"></param>
        /// <param name="key"></param>
        /// <param name="proecessFunc"></param>
        public static TResult AutoProcessProperty<T, TResult>(this HashDictionary hashModel, string key, Func<T, TResult> proecessFunc)
        {
            var value = hashModel.GetValueAs<T>(key);
            if (value != null && hashModel.CheckChanged(key))
            {
                return proecessFunc(value);
            }
            return default(TResult);
        }
    }

    #endregion
}
