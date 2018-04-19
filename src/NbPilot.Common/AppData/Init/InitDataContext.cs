using System;
using System.Collections.Generic;
using System.IO;

namespace NbPilot.Common.AppData.Init
{
    /// <summary>
    /// 预置数据上下文
    /// </summary>
    public interface IInitDataContext
    {
        /// <summary>
        /// 预置数据读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IList<T> Read<T>();
        /// <summary>
        /// 预置数据保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        void Save<T>(IList<T> list);
    }

    /// <summary>
    /// 预置数据上下文
    /// </summary>
    public class InitDataContext : IInitDataContext
    {
        public InitDataContext()
        {
            FileDbHelper = AppData.FileDbHelper.Resolve();
            TypeFilePathHelper = AppData.TypeFilePathHelper.Resolve();
            AppFolderPath = AppData.AppFolderPath.Resolve();
        }

        /// <summary>
        /// 预置数据读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> Read<T>()
        {
            string filePath = MakeInitDataFilePath(typeof(T));
            var instance = FileDbHelper.Read<T>(filePath);
            return instance;
        }

        /// <summary>
        /// 预置数据保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void Save<T>(IList<T> list)
        {
            string filePath = MakeInitDataFilePath(typeof(T));
            FileDbHelper.Save(filePath, list);
        }

        /// <summary>
        /// 
        /// </summary>
        public IFileDbHelper FileDbHelper { get; set; }
        /// <summary>
        /// 初始化数据文件的路径帮助类
        /// </summary>
        public ITypeFilePathHelper TypeFilePathHelper { get; set; }
        /// <summary>
        /// 应用文件夹帮助类
        /// </summary>
        public IAppFolderPath AppFolderPath { get; set; }

        /// <summary>
        /// 默认的路径生成
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string MakeInitDataFilePath(Type type)
        {
            var initFolder = GetInitFolder();
            var filePath = TypeFilePathHelper.AutoGuessTypeFilePath(initFolder, type);
            return filePath;
        }

        private string _initFolder = null;
        /// <summary>
        /// 获取存放的文件夹路径
        /// </summary>
        /// <returns></returns>
        private string GetInitFolder()
        {
            if (_initFolder != null)
            {
                return _initFolder;
            }
            var appData = AppFolderPath.AppData;
            _initFolder = AppFolderPath.CombinePath(appData, "Init");
            return _initFolder;
        }

        #region for di extensions

        private static Func<IInitDataContext> _resolve = () => ResolveAsSingleton.Resolve<InitDataContext, IInitDataContext>();

        public static Func<IInitDataContext> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

    }
}
