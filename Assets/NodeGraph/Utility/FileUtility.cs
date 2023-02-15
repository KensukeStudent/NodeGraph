using System.IO;
using UnityEngine;

namespace NodeGraph
{
    public class FileUtility
    {
        /// <summary>
        /// 入力されたファイル名のAssets以下のファイルパスを取得
        /// </summary>
        public static string GetFilePath(string fileName)
        {
            var paths = Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories);
            if (paths != null && paths.Length > 0)
            {
                return paths[0].Replace("\\", "/").Replace(Application.dataPath, "Assets");
            }
            return null;
        }
    }
}