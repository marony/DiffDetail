using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
    static class ArrayExtention
    {
        public static string ToPrettyString<T>(this T[] array)
        {
            var str = new StringBuilder();
            str.Append("{");
            foreach (var c in array)
            {
                str.Append(c.ToString());
                str.Append(",");
            }
            str.Append("}");
            return str.ToString();
        }
    }

    /// <summary>
    /// 雑多なユーティリティ
    /// </summary>
	public static class Util
	{
		/// <summary>
		/// パスの最後に付いているセパレータを取り除く
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string RemoveEndSeparator(string path)
		{
			if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				path = path.Substring(0, path.Length - 1);
			return path;
		}

		/// <summary>
		/// 指定されたパスから指定されたパターンでサブディレクトリまでファイルを検索
		/// </summary>
		/// <param name="path"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static string[] GetFiles(string path, string[] patterns)
		{
			try
			{
                var files = new SortedDictionary<string, string>();
			    foreach (var pattern in patterns)
			    {
			        var files2 = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
			        foreach (var file in files2)
			        {
			            if (!files.ContainsKey(file))
			                files.Add(file, string.Empty);
			        }
			    }
			    return files.Keys.ToArray();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("ERROR:{0},{1},{2}", ex, path, patterns.ToPrettyString());
				return new string[0];
			}
		}

		/// <summary>
		/// 二つのディレクトリのファイルたちをマージする
		/// </summary>
		/// <param name="lhsPath"></param>
		/// <param name="rhsPath"></param>
		/// <param name="lhsFiles"></param>
		/// <param name="rhsFiles"></param>
		/// <returns></returns>
		public static Dictionary<string, Tuple<string, string>> MergeFiles(string lhsPath, string rhsPath, string[] lhsFiles, string[] rhsFiles)
		{
			var fileMap = new Dictionary<string, Tuple<string, string>>();
			// 元にいるファイル
			foreach (var file in lhsFiles)
			{
				var name = file.Substring(lhsPath.Length + 1);
				fileMap.Add(name, Tuple.Create<string, string>(file, null));
			}
			foreach (var file in rhsFiles)
			{
				var name = file.Substring(rhsPath.Length + 1);
				Tuple<string, string> tuple = null;
				if (fileMap.TryGetValue(name, out tuple)) // 双方にいるファイル
					fileMap[name] = Tuple.Create(tuple.Item1, file);
				else // 先にだけいるファイル
					fileMap.Add(name, Tuple.Create<string, string>(null, file));
			}
			return fileMap;
		}
        /// <summary>
        /// 比較結果をコンソールに出力
        /// </summary>
        /// <param name="key"></param>
        /// <param name="diff"></param>
		public static void Output(string key, List<DiffResult> diff)
		{
			var add = 0;
			var remove = 0;
			var lstr = new StringBuilder();
			var rstr = new StringBuilder();
			for (var i = 0; i < diff.Count; ++i)
			{
				var m = diff[i];
				if (m.Diff == Difference.Add)
					++add;
				else
					++remove;
				if (i > 0)
				{
					var p = diff[i - 1];
					if (p.Lhs != null && m.Lhs != null)
						lstr.AppendLine();
					if (p.Rhs != null && m.Rhs != null)
						rstr.AppendLine();
				}
				if (m.Lhs != null)
					lstr.Append(m.Lhs);
				if (m.Rhs != null)
					rstr.Append(m.Rhs);
			}
			// Excelが改行付きのCSVを読み込めるように'"'を'""'に変換
            var lstr2 = lstr.ToString();
            if (lstr2.Length > 1024)
                lstr2 = lstr2.Substring(0, 1024);
            var rstr2 = rstr.ToString();
            if (rstr2.Length > 1024)
                rstr2 = rstr2.Substring(0, 1024);
            lstr2 = lstr2.Replace("\"", "\"\"");
			rstr2 = rstr2.Replace("\"", "\"\"");
			Console.WriteLine("\"{0}\",{1},{2},\"{3}\",\"{4}\"",
				key, add, remove,
				lstr2, rstr2);
		}
	}
}
