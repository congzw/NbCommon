using System;
using System.IO;

namespace NbPilot.Common
{
    public interface IMyAppPath
    {
        bool IsWebApp { get; set; }
        string BaseDirectory { get; set; }
        string Bin { get; set; }
        string AppData { get; set; }
        string CombinePath(string basePath, string subPath);
    }

    public class MyAppPath : IMyAppPath
    {
        public MyAppPath()
        {
            IsWebApp = GuessIsWebApp();
            Bin = GuessBin(IsWebApp);
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppData = CombinePath(BaseDirectory, "App_Data");
        }

        public bool IsWebApp { get; set; }
        public string BaseDirectory { get; set; }
        public string Bin { get; set; }
        public string AppData { get; set; }
        public string CombinePath(string basePath, string subPath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                throw new ArgumentNullException("basePath");
            }

            if (string.IsNullOrWhiteSpace(subPath))
            {
                throw new ArgumentNullException("subPath");
            }
            return Path.Combine(basePath, subPath);
        }

        private string GuessBin(bool isWebApp)
        {
            return !isWebApp ? AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.RelativeSearchPath;
        }

        private bool GuessIsWebApp()
        {
            return !String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath);
        }

        #region for di extensions

        private static Func<IMyAppPath> _resolve = () => ResolveAsSingleton.Resolve<MyAppPath, IMyAppPath>();
        public static Func<IMyAppPath> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}