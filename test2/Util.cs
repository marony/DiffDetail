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
			for (var i = 0; i < lhsFiles.Length; ++i)
			{
				var file = lhsFiles[i];
				var name = file.Substring(lhsPath.Length + 1);
				fileMap.Add(name, Tuple.Create<string, string>(file, null));
			}
			for (var i = 0; i < rhsFiles.Length; ++i)
			{
				var file = rhsFiles[i];
				var name = file.Substring(rhsPath.Length + 1);
				Tuple<string, string> tuple = null;
				if (fileMap.TryGetValue(name, out tuple)) // 双方にいるファイル
					fileMap[name] = Tuple.Create(tuple.Item1, file);
				else // 先にだけいるファイル
					fileMap.Add(name, Tuple.Create<string, string>(null, file));
			}
			return fileMap;
		}
