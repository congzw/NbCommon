using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace NbPilot.Common
{
    public interface IHashDictionary
    {
        Guid GetCurrentVersion();
        IList<Guid> GetVersions();
        IDictionary<string, HashItem> GetHashValues();
        IDictionary<string, object> GetProperties();
        string GetHashValue(string key);
        T GetValueAs<T>(string key);
    }

    public class HashItem
    {
        public string Key { get; set; }
        public string Hash { get; set; }
        public Guid Version { get; set; }
    }

    public class HashDictionary : IDictionary<string, object>, IHashDictionary
    {
        private readonly Guid _currentVersion = Guid.NewGuid();
        public static readonly string VersionsKey = "_Versions";
        public static readonly string HashValuesKey = "_HashValues";
        //public Dictionary<string, string> _initHashValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, object> _innerDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public HashDictionary()
        {
            _innerDictionary.Add(VersionsKey, new List<Guid>(){_currentVersion});
            _innerDictionary.Add(HashValuesKey, new Dictionary<string, HashItem>(StringComparer.OrdinalIgnoreCase));
            //_innerDictionary.Add(HashValuesKey, _initHashValues);
        }

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
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
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
            if (key == HashValuesKey)
            {
                //not allowed add _HashValues by user!
                throw new InvalidOperationException("This key is keep by design, not allowed use directly: " + HashValuesKey);
            }
            TrySetKeyValueHash(key, value);
            TrySetKeyValue(key, value);
        }

        public bool Remove(string key)
        {
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
                if (key == VersionsKey)
                {
                    //Console.WriteLine("FROM INDEX[]");
                    ProcessSerializeVersions(value);
                    return;
                }
                if (key == HashValuesKey)
                {
                    //Console.WriteLine("FROM INDEX[]");
                    ProcessSerializeHashValues(value);
                    return;
                }
                TrySetKeyValueHash(key, value);
                TrySetKeyValue(key, value);
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

        //helpers
        private void ProcessSerializeHashValues(object value)
        {
            //this can only be invoked from json deserialize, so replace the process init hash values
            //Console.WriteLine(value);
            var jObject = (JObject)value;
            var hashValues = jObject.ToObject<IDictionary<string, HashItem>>();

            //Console.WriteLine("Hash From JSON");
            //foreach (var hashValueKey in hashValues.Keys)
            //{
            //    var hashValue = hashValues[hashValueKey];
            //    Console.WriteLine("{0}: {1} => {2}", hashValueKey, hashValue.Hash, hashValue.Version);
            //}
            _innerDictionary[HashValuesKey] = hashValues;

            #region trace info

            //Console.WriteLine("RESET ALL HASH VALUES WITH JSON! TODO");
            //foreach (var key in hashValues.Keys)
            //{
            //    Console.WriteLine("{0}: {1}", key, hashValues[key]);
            //}

            //Console.WriteLine(value.GetType());
            //Console.WriteLine("CURRENT DICTIONARY: ");
            //foreach (var key in _innerDictionary.Keys)
            //{
            //    Console.WriteLine("{0}: {1}", key, _innerDictionary[key]);
            //}

            //var hashValues = (IDictionary<string, string>)_innerDictionary[HashValuesKey];
            ////Console.WriteLine(hashValues);
            //foreach (var hashValueKey in hashValues.Keys)
            //{
            //    Console.WriteLine("{0}: {1}", hashValueKey, hashValues[hashValueKey]);
            //}

            #endregion
        }

        private void ProcessSerializeVersions(object value)
        {
            //this can only be invoked from json deserialize
            //Console.WriteLine(value);
            var jObject = (JArray)value;
            var jsonVersions = jObject.ToObject<List<Guid>>();

            //Console.WriteLine("Versions From JSON: ");
            //foreach (var @version in jsonVersions)
            //{
            //    Console.WriteLine(@version);
            //}
            var currentVersions = GetVersions();
            foreach (var jsonVersion in jsonVersions)
            {
                if (!currentVersions.Contains(jsonVersion))
                {
                    currentVersions.Add(jsonVersion);
                }
            }
            _innerDictionary[VersionsKey] = currentVersions;
        }
        private void TrySetKeyValueHash(string key, object value)
        {
            if (key == HashValuesKey)
            {
                return;
            }
            var initHashValues = GetHashValues();
            if (initHashValues.ContainsKey(key))
            {
                return;
            }
            initHashValues.Add(key, new HashItem() { Key = key, Hash = value.CreateObjectHash() , Version = GetCurrentVersion()});
        }
        private void TrySetKeyValue(string key, object value)
        {
            if (key == HashValuesKey)
            {
                return;
            }
            _innerDictionary[key] = value;
        }

        #region IHashDictionary

        public Guid GetCurrentVersion()
        {
            return _currentVersion;
        }

        public IList<Guid> GetVersions()
        {
            return (List<Guid>)_innerDictionary[VersionsKey];
        }

        public IDictionary<string, HashItem> GetHashValues()
        {
            return (Dictionary<string, HashItem>)_innerDictionary[HashValuesKey];
            //return _initHashValues;
        }

        public IDictionary<string, object> GetProperties()
        {
            return _innerDictionary;
        }

        public string GetHashValue(string key)
        {
            var hashValues = GetHashValues();
            if (!hashValues.ContainsKey(key))
            {
                return string.Empty;
            }
            return hashValues[key].Hash;
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
        public static bool CheckChanged(this IHashDictionary hashModel, string key)
        {
            if (hashModel == null || key == null)
            {
                return false;
            }

            var hashValues = hashModel.GetHashValues();
            if (!hashValues.ContainsKey(key))
            {
                return false;
            }
            var hashValue = hashValues[key];
            var properties = hashModel.GetProperties();
            if (!properties.ContainsKey(key))
            {
                //be removed!
                return true;
            }

            var sameHash = properties[key].VerifyObjectHash(hashValue.Hash);
            if (!sameHash)
            {
                //hash不一样，一定变化
                return true;
            }

            //哈希值虽然一样，但只要不是初始版本的，都视为变化（新增）
            var initVersion = hashModel.GetVersions().LastOrDefault();
            return initVersion != hashValue.Version;
        }

        /// <summary>
        /// 检测是否有任何项改变
        /// </summary>
        /// <param name="hashModel"></param>
        /// <returns></returns>

        public static bool CheckAnyChanged(this IHashDictionary hashModel)
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
        public static void ShouldNotNull(this IHashDictionary hashModel)
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
        public static IHashDictionary AutoAddOrUpdate<T>(this IHashDictionary hashModel, string key, Func<string, bool> shouldInclude, Func<T> getFunc)
        {
            if (shouldInclude(key))
            {
                var value = getFunc();
                var properties = hashModel.GetProperties();
                properties[key] = value;
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
        public static void AutoProcessProperty<T>(this IHashDictionary hashModel, string key, Action<T> proecessAction)
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
        public static TResult AutoProcessProperty<T, TResult>(this IHashDictionary hashModel, string key, Func<T, TResult> proecessFunc)
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

    #region version
    //using System;
    //using System.Collections;
    //using System.Collections.Generic;
    //using System.Linq;
    //using Newtonsoft.Json;
    //using Newtonsoft.Json.Linq;

    //// ReSharper disable CheckNamespace

    //namespace NbPilot.Common
    //{
    //    //public interface IHashDictionary2
    //    //{
    //    //    IDictionary<string, string> HashValues { get; set; }
    //    //    [JsonExtensionData]
    //    //    IDictionary<string, object> Properties { get; set; }
    //    //    string GetPropertyHashValue(string key);
    //    //    T GetPropertyAs<T>(string key);
    //    //}


    //    //public class HashDictionary2
    //    //{
    //    //    public IDictionary<string, string> HashValues { get; set; }
    //    //    public IDictionary<string, object> Properties { get; set; }
    //    //}

    //    public interface IHashDictionary
    //    {
    //        IDictionary<string, HashItem> GetHashValues();
    //        IDictionary<string, object> GetProperties();
    //        string GetHashValue(string key);
    //        T GetValueAs<T>(string key);
    //        Guid GetCurrentVersion();
    //    }

    //    public class HashItem
    //    {
    //        public string Key { get; set; }
    //        public string Hash { get; set; }
    //        public Guid Version { get; set; }
    //    }

    //    public class HashDictionary : IDictionary<string, object>, IHashDictionary
    //    {
    //        public static readonly string HashValuesKey = "_HashValues";
    //        public readonly Guid HashValuesVersion = Guid.NewGuid();
    //        public Dictionary<string, HashItem> _initHashValues = new Dictionary<string, HashItem>(StringComparer.OrdinalIgnoreCase);
    //        private readonly Dictionary<string, object> _innerDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    //        public HashDictionary()
    //        {
    //            //_innerDictionary.Add(HashValuesKey, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
    //            _innerDictionary.Add(HashValuesKey, _initHashValues);
    //        }

    //        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    //        {
    //            return _innerDictionary.GetEnumerator();
    //        }

    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return GetEnumerator();
    //        }

    //        public void Add(KeyValuePair<string, object> item)
    //        {
    //            Add(item.Key, item.Value);
    //        }

    //        public void Clear()
    //        {
    //            _innerDictionary.Clear();
    //        }

    //        public bool Contains(KeyValuePair<string, object> item)
    //        {
    //            return ((IDictionary<string, object>)_innerDictionary).Contains(item);
    //        }

    //        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    //        {
    //            ((IDictionary<string, object>)_innerDictionary).CopyTo(array, arrayIndex);
    //        }

    //        public bool Remove(KeyValuePair<string, object> item)
    //        {
    //            return ((IDictionary<string, object>)_innerDictionary).Remove(item);
    //        }

    //        public int Count
    //        {
    //            get { return _innerDictionary.Count; }
    //        }

    //        public bool IsReadOnly
    //        {
    //            get { return ((IDictionary<string, object>)_innerDictionary).IsReadOnly; }
    //        }

    //        public bool ContainsKey(string key)
    //        {
    //            return _innerDictionary.ContainsKey(key);
    //        }

    //        public void Add(string key, object value)
    //        {
    //            if (key == HashValuesKey)
    //            {
    //                //not allowed add _HashValues by user!
    //                throw new InvalidOperationException("This key is keep by design, not allowed use directly: " + HashValuesKey);
    //            }
    //            TrySetKeyValue(key, value);
    //            TrySetKeyValueHash(key, value);
    //        }

    //        public bool Remove(string key)
    //        {
    //            return _innerDictionary.Remove(key);
    //        }

    //        public bool TryGetValue(string key, out object value)
    //        {
    //            return _innerDictionary.TryGetValue(key, out value);
    //        }

    //        public object this[string key]
    //        {
    //            get
    //            {
    //                object value;
    //                _innerDictionary.TryGetValue(key, out value);
    //                return value;
    //            }
    //            set
    //            {
    //                //_innerDictionary[key] = value;
    //                if (key == HashValuesKey)
    //                {
    //                    //Console.WriteLine("FROM INDEX[]");
    //                    ProcessSerializeHashValues(value);
    //                    return;
    //                }
    //                TrySetKeyValue(key, value);
    //                TrySetKeyValueHash(key, value);
    //            }
    //        }
    //        public ICollection<string> Keys
    //        {
    //            get { return _innerDictionary.Keys; }
    //        }
    //        public ICollection<object> Values
    //        {
    //            get { return _innerDictionary.Values; }
    //        }

    //        //helpers
    //        private void ProcessSerializeHashValues(object value)
    //        {
    //            //this can only be invoked from json deserialize, so replace the process init hash values
    //            //Console.WriteLine(value);
    //            var jObject = (JObject)value;
    //            var hashValues = jObject.ToObject<IDictionary<string, HashItem>>();
    //            _innerDictionary[HashValuesKey] = hashValues;

    //            #region trace info

    //            //Console.WriteLine("RESET ALL HASH VALUES WITH JSON! TODO");
    //            //foreach (var key in hashValues.Keys)
    //            //{
    //            //    Console.WriteLine("{0}: {1}", key, hashValues[key]);
    //            //}

    //            //Console.WriteLine(value.GetType());
    //            //Console.WriteLine("CURRENT DICTIONARY: ");
    //            //foreach (var key in _innerDictionary.Keys)
    //            //{
    //            //    Console.WriteLine("{0}: {1}", key, _innerDictionary[key]);
    //            //}

    //            //var hashValues = (IDictionary<string, string>)_innerDictionary[HashValuesKey];
    //            ////Console.WriteLine(hashValues);
    //            //foreach (var hashValueKey in hashValues.Keys)
    //            //{
    //            //    Console.WriteLine("{0}: {1}", hashValueKey, hashValues[hashValueKey]);
    //            //}

    //            #endregion
    //        }
    //        private void TrySetKeyValueHash(string key, object value)
    //        {
    //            if (key == HashValuesKey)
    //            {
    //                return;
    //            }
    //            var initHashValues = GetHashValues();
    //            if (initHashValues.ContainsKey(key))
    //            {
    //                return;
    //            }
    //            initHashValues.Add(key, new HashItem() { Key = key, Hash = value.CreateObjectHash() , Version = GetCurrentVersion() });
    //        }
    //        private void TrySetKeyValue(string key, object value)
    //        {
    //            if (key == HashValuesKey)
    //            {
    //                return;
    //            }

    //            _innerDictionary[key] = value;
    //        }

    //        #region IHashDictionary

    //        public IDictionary<string, HashItem> GetHashValues()
    //        {
    //            return _initHashValues;
    //        }

    //        public IDictionary<string, object> GetProperties()
    //        {
    //            return _innerDictionary;
    //        }

    //        public string GetHashValue(string key)
    //        {
    //            var hashValues = GetHashValues();
    //            if (!hashValues.ContainsKey(key))
    //            {
    //                return string.Empty;
    //            }
    //            return hashValues[key].Hash;
    //        }

    //        public T GetValueAs<T>(string key)
    //        {
    //            if (!this.ContainsKey(key))
    //            {
    //                return default(T);
    //            }
    //            return (T)this[key];
    //        }

    //        public Guid GetCurrentVersion()
    //        {
    //            return HashValuesVersion;
    //        }

    //        #endregion
    //    }

    //    #region extensions

    //    /// <summary>
    //    /// DictionaryWithHash Extensions
    //    /// </summary>
    //    public static class HashModelExtensions
    //    {
    //        /// <summary>
    //        /// 检测是否某一项改变
    //        /// </summary>
    //        /// <param name="hashModel"></param>
    //        /// <param name="key"></param>
    //        /// <returns></returns>
    //        public static bool CheckChanged(this IHashDictionary hashModel, string key)
    //        {
    //            if (hashModel == null || key == null)
    //            {
    //                return false;
    //            }

    //            var hashValues = hashModel.GetHashValues();
    //            if (!hashValues.ContainsKey(key))
    //            {
    //                return false;
    //            }


    //            var hashValue = hashValues[key];
    //            var properties = hashModel.GetProperties();
    //            if (!properties.ContainsKey(key))
    //            {
    //                //be removed! or different version(after a serilize)
    //                return true;
    //            }

    //            //var currentVersion = hashModel.GetCurrentVersion();
    //            //if (currentVersion != hashValue.Version)
    //            //{
    //            //    //not current version
    //            //    return true;
    //            //}

    //            IList<Guid> allVersions = hashValues.Values.Select(x => x.Version).Distinct().ToList();
    //            if (allVersions.Count > 1)
    //            {
    //                var lastVersion = allVersions.Last();
    //                if (lastVersion == hashValue.Version)
    //                {
    //                    //多个版本，并且是最后一个，一定是新增的
    //                    return true;
    //                }
    //            }

    //            return !properties[key].VerifyObjectHash(hashValue.Hash);
    //        }

    //        /// <summary>
    //        /// 检测是否有任何项改变
    //        /// </summary>
    //        /// <param name="hashModel"></param>
    //        /// <returns></returns>

    //        public static bool CheckAnyChanged(this IHashDictionary hashModel)
    //        {
    //            if (hashModel == null)
    //            {
    //                return false;
    //            }

    //            var hashValues = hashModel.GetHashValues();
    //            foreach (var hashValue in hashValues)
    //            {
    //                var changedForKey = CheckChanged(hashModel, hashValue.Key);
    //                if (changedForKey)
    //                {
    //                    return true;
    //                }
    //            }

    //            return false;
    //        }

    //        /// <summary>
    //        /// 不能为空
    //        /// </summary>
    //        /// <param name="hashModel"></param>
    //        public static void ShouldNotNull(this IHashDictionary hashModel)
    //        {
    //            if (hashModel == null)
    //            {
    //                throw new ArgumentNullException("hashModel");
    //            }
    //        }

    //        /// <summary>
    //        /// 按照指示决定是否添加值到属性中
    //        /// </summary>
    //        /// <typeparam name="T"></typeparam>
    //        /// <param name="hashModel"></param>
    //        /// <param name="key"></param>
    //        /// <param name="shouldInclude"></param>
    //        /// <param name="getFunc"></param>
    //        /// <returns></returns>
    //        public static IHashDictionary AutoAddOrUpdate<T>(this IHashDictionary hashModel, string key, Func<string, bool> shouldInclude, Func<T> getFunc)
    //        {
    //            if (shouldInclude(key))
    //            {
    //                var value = getFunc();
    //                var properties = hashModel.GetProperties();
    //                properties[key] = value;
    //            }
    //            return hashModel;
    //        }

    //        /// <summary>
    //        /// 按照是否包含并且变化决定调用处理程序（如果模型中确实包含某属性，并且确实检测到变化，才调用处理程序）
    //        /// </summary>
    //        /// <typeparam name="T"></typeparam>
    //        /// <param name="hashModel"></param>
    //        /// <param name="key"></param>
    //        /// <param name="proecessAction"></param>
    //        public static void AutoProcessProperty<T>(this IHashDictionary hashModel, string key, Action<T> proecessAction)
    //        {
    //            var value = hashModel.GetValueAs<T>(key);
    //            if (value != null && hashModel.CheckChanged(key))
    //            {
    //                proecessAction(value);
    //            }
    //        }

    //        /// <summary>
    //        /// 按照是否包含并且变化决定调用处理程序（如果模型中确实包含某属性，并且确实检测到变化，才调用处理程序）
    //        /// </summary>
    //        /// <typeparam name="T"></typeparam>
    //        /// <typeparam name="TResult"></typeparam>
    //        /// <param name="hashModel"></param>
    //        /// <param name="key"></param>
    //        /// <param name="proecessFunc"></param>
    //        public static TResult AutoProcessProperty<T, TResult>(this IHashDictionary hashModel, string key, Func<T, TResult> proecessFunc)
    //        {
    //            var value = hashModel.GetValueAs<T>(key);
    //            if (value != null && hashModel.CheckChanged(key))
    //            {
    //                return proecessFunc(value);
    //            }
    //            return default(TResult);
    //        }
    //    }

    //    #endregion
    //}


    #endregion
}
