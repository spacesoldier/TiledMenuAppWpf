using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TiledMenuAppWpf
{
    class FileLoadHelper
    {
        public static List<FileInfo> findPictures(string dirName)
        {
            List<FileInfo> result = new List<FileInfo>();
            
            DirectoryInfo dir = new DirectoryInfo(dirName);

            FileInfo[] files = dir.GetFiles("*.*");

            foreach (FileInfo f in files)
            {
                FileInfo file = new FileInfo(f.FullName);
                result.Add(file);
            }

            return result;
        }
    }
}
