using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// 簡易Diffロジッククラス
	/// </summary>
	class SimpleDiffLogic : DiffLogic
	{
		public SimpleDiffLogic(Splitter splitter)
			: base(splitter)
		{
			
		}

		public override IEnumerable<DiffResult> Diff(string lhs, string rhs)
		{
			// 文字列を分解
			var s_l = _splitter.Split(lhs).ToList();
			var s_r = _splitter.Split(rhs).ToList();

			var editGraph = CreateEditGraph(s_l, s_r);

			var diffResults = new List<DiffResult>();
			var x = s_l.Count;
			var y = s_r.Count;
			while (x > 0 || y > 0)
			{
				var l = (x > 0 ? s_l[x - 1] : "");
				var r = (y > 0 ? s_r[y - 1] : "");

				var cost_up = (y <= 0 ? int.MaxValue : editGraph[y - 1][x]);
				var cost_left = (x <= 0 ? int.MaxValue : editGraph[y][x - 1]);
				var cost_upleft = (x <= 0 || y <= 0 || l != r ? int.MaxValue : editGraph[y - 1][x - 1]);
				if (cost_upleft <= cost_up)
				{
					if (cost_upleft <= cost_left)
					{
						--y;
						--x;
						var diff = new DiffResult(Difference.Same, l, r);
						diffResults.Add(diff);
					}
					else
					{
						--x;
						var diff = new DiffResult(Difference.Remove, l, null);
						diffResults.Add(diff);
					}
				}
				else
				{
					if (cost_up <= cost_left)
					{
						--y;
						var diff = new DiffResult(Difference.Add, null, r);
						diffResults.Add(diff);
					}
					else
					{
						--x;
						var diff = new DiffResult(Difference.Remove, l, null);
						diffResults.Add(diff);
					}
				}
			}
			diffResults.Reverse();
			return diffResults;
		}

		protected List<List<int>> CreateEditGraph(List<string> lhs, List<string> rhs)
		{
			// エディットグラフ作成
			var editGraph = new List<List<int>>();
			for (var y = 0; y <= rhs.Count; ++y)
			{
				var row = new List<int>();
				editGraph.Add(row);
				for (var x = 0; x <= lhs.Count; ++x)
				{
					var c = 0;
					if (x == 0 && y == 0)
						c = 0;
					else if (x == 0)
						c = y;
					else if (y == 0)
						c = x;
					else
					{
						var l = lhs[x - 1];
						var r = rhs[y - 1];
						var cost_up = editGraph[y - 1][x] + 1;
						var cost_left = editGraph[y][x - 1] + 1;
						var min = Math.Min(cost_up, cost_left);
						if (l == r)
							min = Math.Min(min, editGraph[y - 1][x - 1]);
						c = min;
					}
					row.Add(c);
				}
			}
			return editGraph;
		}
	}
}
