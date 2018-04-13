using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NbPilot.Common
{
    public class FileHelper
    {
        //从指定的路径下，查找文件
        private List<string> FindFiles(string folderPath, string match, SearchOption searchOption = SearchOption.TopDirectoryOnly, string[] excludeFiles = null)
        {
            if (string.IsNullOrWhiteSpace(match))
            {
                throw new ArgumentNullException("match");
            }

            var files = GetFiles(folderPath, match, searchOption);
            if (excludeFiles != null)
            {
                files = files.Where(file => !excludeFiles.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToList();
            }
            return files;
        }
        //查找所有文件
        private List<string> GetFiles(string folderPath, string searchPattern, SearchOption searchOption)
        {
            List<string> list = new List<string>();
            string[] temp = Directory.GetFiles(folderPath, searchPattern, searchOption);
            if (temp.Length > 0)
            {
                list.AddRange(temp);
            }
            return list;
        }
    }
}
