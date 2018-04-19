using System;
using System.IO;

namespace NbPilot.Common.AppData
{
    /// <summary>
    /// 初始化数据用的JsonFilePath
    /// </summary>
    public interface ITypeFilePathHelper
    {
        /// <summary>
        /// 文件后缀
        /// </summary>
        string Suffix { get; set; }

        /// <summary>
        /// 默认的路径生成
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="type"></param>
        /// <param name="customizeName"></param>
        /// <returns></returns>
        string AutoGuessTypeFilePath(string folderPath, Type type, string customizeName = null);
    }

    public class TypeFilePathHelper : ITypeFilePathHelper
    {
        public TypeFilePathHelper()
        {
            Suffix = ".json";
        }

        public string Suffix { get; set; }

        public string AutoGuessTypeFilePath(string folderPath, Type type, string customizeName = null)
        {
            string fileNameFix = MakeUniqueName(type, customizeName);

            var extension = Path.GetExtension(fileNameFix);
            if (string.IsNullOrWhiteSpace(extension))
            {
                fileNameFix = fileNameFix + "." + Suffix.TrimStart('.');
            }

            var filePath = Path.Combine(folderPath, fileNameFix);
            return filePath;
        }

        protected virtual string MakeUniqueName(Type type, string customizeTypeName = null)
        {
            string fileNameFix = type.Name;
            if (!string.IsNullOrWhiteSpace(customizeTypeName))
            {
                fileNameFix = customizeTypeName;
            }
            return fileNameFix;
        }
        
        #region for di extensions

        private static Func<ITypeFilePathHelper> _resolve = () => new TypeFilePathHelper();
        public static Func<ITypeFilePathHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}
