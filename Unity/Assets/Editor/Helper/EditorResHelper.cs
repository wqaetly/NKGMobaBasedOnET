using System.Collections.Generic;
using System.IO;

namespace ETModel
{
    public class EditorResHelper
    {
        /// <summary>
        /// 获取文件夹内所有资源路径
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <param name="subDire">是否获取子文件夹</param>
        /// <returns></returns>
        public static List<string> GetAllResourcePath(string srcPath, bool subDire)
        {
            List<string> paths = new List<string>();
            string[] files = Directory.GetFiles(srcPath);
            foreach (string str in files)
            {
                if (str.EndsWith(".meta"))
                {
                    continue;
                }

                paths.Add(str);
            }

            if (subDire)
            {
                foreach (string subPath in Directory.GetDirectories(srcPath))
                {
                    List<string> subFiles = GetAllResourcePath(subPath, true);
                    paths.AddRange(subFiles);
                }
            }

            return paths;
        }

        /// <summary>
        /// 获取文件夹内所有dds资源路径(包含子文件夹)
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <returns></returns>
        public static List<string> GetAllDDSFilePath(string srcPath)
        {
            List<string> paths = new List<string>();
            string[] files = Directory.GetFiles(srcPath);
            foreach (string str in files)
            {
                if (str.EndsWith(".dds"))
                {
                    paths.Add(str);
                }
            }

            foreach (string subPath in Directory.GetDirectories(srcPath))
            {
                List<string> subFiles = GetAllResourcePath(subPath, true);
                paths.AddRange(subFiles);
            }

            return paths;
        }
    }
}