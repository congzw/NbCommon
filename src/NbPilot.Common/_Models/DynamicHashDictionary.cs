using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
// ReSharper disable CheckNamespace

namespace NbPilot.Common
{
    public class DynamicHashDictionary : DynamicObject
    {
        private readonly HashDictionary _hashData;

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

        // Implementing this function improves the debugging experience as it provides the debugger with the list of all
        // the properties currently defined on the object
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return DynamicHashData.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = DynamicHashData[binder.Name];
            // since always returns a result even if the key does not exist, always return true
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            DynamicHashData[binder.Name] = value;
            // you can always set a key in the dictionary so return true
            return true;
        }
        
        /// <summary>
        /// Factory
        /// </summary>
        /// <returns></returns>
        public static DynamicHashDictionary Create()
        {
            return new DynamicHashDictionary(new HashDictionary());
        }

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
        
        /// <summary>
        /// 按照指示决定是否添加值到属性中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="shouldInclude"></param>
        /// <param name="getFunc"></param>
        /// <returns></returns>
        public DynamicHashDictionary AutoAddOrUpdate<T>(string key, Func<string, bool> shouldInclude, Func<T> getFunc)
        {
            var hashModel = DynamicHashData;
            hashModel.AutoAddOrUpdate(key, shouldInclude, getFunc);
            return this;
        }

        /// <summary>
        /// 按照是否包含并且变化决定调用处理程序（如果模型中确实包含某属性，并且确实检测到变化，才调用处理程序）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="proecessAction"></param>
        public void AutoProcessProperty<T>(string key, Action<T> proecessAction)
        {
            var hashModel = DynamicHashData;
            hashModel.AutoProcessProperty(key, proecessAction);
        }

        /// <summary>
        /// 按照是否包含并且变化决定调用处理程序（如果模型中确实包含某属性，并且确实检测到变化，才调用处理程序）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="proecessFunc"></param>
        public TResult AutoProcessProperty<T, TResult>(string key, Func<T, TResult> proecessFunc)
        {
            var hashModel = DynamicHashData;
            return hashModel.AutoProcessProperty(key, proecessFunc);
        }

        #endregion
    }
}