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
				Console.WriteLine("USAGE: DiffDetail SrcDir DestDir [Pattern]");
				return;
			}

			// オプションの処理
			var lhsPath = Util.RemoveEndSeparator(args[0]);
			var rhsPath = Util.RemoveEndSeparator(args[1]);
			var pattern = "*.*";
			if (args.Length >= 3)
				pattern = args[2];

			Console.WriteLine("{0},{1},{2}", lhsPath, rhsPath, pattern);

			// 元と先のディレクトリからファイルを検索
			var lhsFiles = Util.GetFiles(lhsPath, pattern);
			var rhsFiles = Util.GetFiles(rhsPath, pattern);

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

					var r = diffLogic.Diff(lhsContent, rhsContent);
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
