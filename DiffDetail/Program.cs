using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("USAGE: DiffDetail SrcDir DestDir [Pattern(カンマ区切り)]");
				return;
			}

			// オプションの処理
			var lhsPath = Util.RemoveEndSeparator(args[0]);
			var rhsPath = Util.RemoveEndSeparator(args[1]);
			var patterns = new string[] { "*.*" };
			if (args.Length >= 3)
				patterns = args[2].Split(',');

			Console.WriteLine("{0},{1},{2}", lhsPath, rhsPath, patterns.ToPrettyString());

			// 元と先のディレクトリからファイルを検索
			var lhsFiles = Util.GetFiles(lhsPath, patterns);
			var rhsFiles = Util.GetFiles(rhsPath, patterns);

			var fileMap = Util.MergeFiles(lhsPath, rhsPath, lhsFiles, rhsFiles);
			// 全てのファイルを比較してCSVファイルを出力
			Console.WriteLine("ファイル名,追加行数,削除行数,前内容,後内容");
			foreach (var key in fileMap.Keys.OrderBy(v => v))
			{
				var tuple = fileMap[key];
				var lhsFileName = tuple.Item1;
				var rhsFileName = tuple.Item2;
				var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Line, (lhsFileName ?? rhsFileName));
				try
				{
					var lhsContent = (lhsFileName != null ? File.ReadAllText(lhsFileName) : "");
					var rhsContent = (rhsFileName != null ? File.ReadAllText(rhsFileName) : "");

					// 行ごとの比較結果のリスト
					var r = diffLogic.Diff(lhsContent, rhsContent);
					// 異なる行から同じ行になるまでかループが終わるまでを出力
					var diff = new List<DiffResult>();
					foreach (var l in r)
					{
						if (diff.Count > 0)
						{
							if (l.Diff == Difference.Same)
							{
								Util.Output(key, diff);
								diff.Clear();
							}
							else
								diff.Add(l);
						}
						else
						{
							if (l.Diff != Difference.Same)
								diff.Add(l);
						}
					}
					if (diff.Count > 0)
						Util.Output(key, diff);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("ERROR:{0},{1},{2}", ex, lhsFileName, rhsFileName);
				}
			}
		}
	}
}
