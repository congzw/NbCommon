using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

// ReSharper disable CheckNamespace

namespace NbPilot.Common
{
    /// <summary>
    /// DynamicHashDictionary
    /// </summary>
    public class DynamicHashDictionary : DynamicObject
    {
        public DynamicHashDictionary()
            : this(new HashDictionary())
        {
        }

        private HashDictionary _hashData;
        public DynamicHashDictionary(HashDictionary hashData)
        {
            if (hashData == null)
            {
                throw new ArgumentNullException("hashData");
            }
            _hashData = hashData;
        }

        public HashDictionary DynamicHashData
        {
            get
            {
                return _hashData;
            }
        }
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            // Implementing this function improves the debugging experience as it provides the debugger with the list of all
            // the properties currently defined on the object
            return DynamicHashData.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var key = binder.Name;
            var shouldInclude = ShouldInclude(key);
            if (shouldInclude)
            {
                var value = DynamicHashData[key];
                if (value != null)
                {
                    if (value is Delegate)
                    {
                        result = (value as Delegate).DynamicInvoke();
                    }
                    else
                    {
                        result = value;
                    }
                }
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var shouldInclude = ShouldInclude(binder.Name);
            if (shouldInclude)
            {
                DynamicHashData[binder.Name] = value;
            }
            return true;
        }

        #region Getter & Setter

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="func"></param>
        public void Set<T>(string name, Func<T> func)
        {
            Set(name, (object)func);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            var shouldInclude = ShouldInclude(name);
            if (shouldInclude)
            {
                if (value != null)
                {
                    var @delegate = value as Delegate;
                    if (@delegate != null)
                    {
                        DynamicHashData[name] = @delegate.DynamicInvoke();
                    }
                    else
                    {
                        DynamicHashData[name] = value;
                    }
                }
            }
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get<T>(string name)
        {
            var shouldInclude = ShouldInclude(name);
            if (!shouldInclude)
            {
                return default(T);
            }

            var value = DynamicHashData[name];
            return value is T ? (T)value : default(T);
        }

        #endregion

        #region for check

        /// <summary>
        /// 检测是否某一项改变
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckChanged(string key)
        {
            var hashModel = DynamicHashData;
            return hashModel.CheckChanged(key);
        }

        /// <summary>
        /// 检测是否有任何项改变
        /// </summary>
        /// <returns></returns>

        public bool CheckAnyChanged()
        {
            var hashModel = DynamicHashData;
            return hashModel.CheckAnyChanged();
        }

        #endregion
        
        #region ShouldInclude

        private bool _includePropertiesInited = false;
        private List<string> _includeProperties = new List<string>() { "*" };

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="includeProperties"></param>
        public void InitIncludeProperties(params string[] includeProperties)
        {
            if (_includePropertiesInited)
            {
                throw new InvalidOperationException("InitIncludeProperties Should Invoke Only Once");
            }
            _includeProperties = includeProperties.ToList();
            _includePropertiesInited = true;
            //clear all set properties before init invoke!
            _hashData = new HashDictionary();
        }

        /// <summary>
        /// 获取当前应该包含的项
        /// </summary>
        /// <returns></returns>
        public IList<string> GetIncludeProperties()
        {
            return _includeProperties;
        }

        /// <summary>
        /// 是否应该包含
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool ShouldInclude(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (HashDictionary.HashValuesKey.NbEquals(name) || HashDictionary.VersionsKey.NbEquals(name))
            {
                return true;
            }

            if (_includeProperties == null || _includeProperties.Count == 0)
            {
                return false;
            }
            if (_includeProperties.Contains("*"))
            {
                return true;
            }
            var isInclude = _includeProperties.Contains(name, StringComparer.OrdinalIgnoreCase);
            return isInclude;
        }

        #endregion

        #region Factory

        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public static DynamicHashDictionary Init(params string[] includeProperties)
        {
            var result = new DynamicHashDictionary();
            result.InitIncludeProperties(includeProperties);
            return result;
        }

        #endregion
    }

    /// <summary>
    /// DynamicHashDictionary Extensions
    /// </summary>
    public static class DynamicHashDictionaryExtensions
    {
        //refact after read this article
        //https://www.thomaslevesque.com/2009/10/08/c-4-0-implementing-a-custom-dynamic-object/

        /// <summary>
        /// As Dynamic
        /// </summary>
        /// <param name="dynamicHashDictionary"></param>
        /// <returns></returns>
        public static dynamic AsDynamic(this DynamicHashDictionary dynamicHashDictionary)
        {
            return dynamicHashDictionary;
        }
    }
}