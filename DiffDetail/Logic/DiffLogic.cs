using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// Diffロジック種類
	/// </summary>
	public enum DiffLogicType : byte
	{
        // 単純なエディットグラフ
		Simple
	}

	/// <summary>
	/// Diff結果の種別
	/// </summary>
	public enum Difference : byte
	{
        // 比較結果が同じ
		Same,
        // 追加された
		Add,
        // 削除された
		Remove
	}

	/// <summary>
	/// Diff結果クラス
	/// </summary>
	public class DiffResult
	{
		public DiffResult(Difference diff, string lhs, string rhs)
		{
			Diff = diff;
			Lhs = lhs;
			Rhs = rhs;
		}

		public Difference Diff { get; private set; }
		public string Lhs { get; private set; }
		public string Rhs { get; private set; }
	}

	/// <summary>
	/// Diffロジック基底クラス
	/// </summary>
	public abstract class DiffLogic
	{
		protected DiffLogic(Splitter splitter)
		{
			_splitter = splitter;
		}

		/// <summary>
		/// ファクトリメソッド
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static DiffLogic CreateDiff(DiffLogicType diffType, SplitType splitType, string fileName)
		{
			var filter = Filter.CreteFilter(fileName);
			var splitter = Splitter.CreateSplitter(splitType, filter);
			if (splitter == null)
				return null;

			switch (diffType)
			{
			case DiffLogicType.Simple:
				return new SimpleDiffLogic(splitter);
			}
			return null;
		}

		/// <summary>
		/// Diff実行
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public abstract IEnumerable<DiffResult> Diff(string lhs, string rhs);

	 	protected Splitter _splitter = null;
	}
}
