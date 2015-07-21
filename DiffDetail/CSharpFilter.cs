using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// C#用フィルタ
	/// - 空行を取り除く
	/// - 前後の空白を取り除く
	/// - コメントを取り除く(1行・複数行)
	/// - 複数の空白をひとつにする
	/// </summary>
	public class CSharpFilter : Filter
	{
		protected bool _inCommant = false;

		protected Regex _dupSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
		protected Regex _lineCommentRegex = new Regex(@"//.*$", RegexOptions.Compiled);
		protected Regex _rangeCommentRegex = new Regex(@"/\*.*?\*/", RegexOptions.Compiled);
		protected Regex _rangeCommentRegexBegin = new Regex(@"/\*.*$", RegexOptions.Compiled);
		protected Regex _rangeCommentRegexEnd = new Regex(@".*\*/", RegexOptions.Compiled);

		public override string FilterElement(string str)
		{
			if (!_inCommant)
			{
				// 1行コメントを削除
				str = _lineCommentRegex.Replace(str, @"");
				// 複数行コメントを削除
				str = _rangeCommentRegex.Replace(str, @" ");
				var m = _rangeCommentRegexBegin.Match(str);
				if (m.Success)
				{
					str = _rangeCommentRegexBegin.Replace(str, @" ");
					_inCommant = true;
				}
				// 前後の空白を削除
				str = str.Trim();
				// 連続した空白を1つの空白に置換
				str = _dupSpaceRegex.Replace(str, @" ");
			}
			else
			{
				var m = _rangeCommentRegexEnd.Match(str);
				if (m.Success)
				{
					str = _rangeCommentRegexEnd.Replace(str, @" ");
					_inCommant = false;
					if (str.Length > 0)
						str = FilterElement(str);
				}
				else
					return null;
			}
			if (str != null && str.Length <= 0)
				return null;
			return str;
		}
	}
}
