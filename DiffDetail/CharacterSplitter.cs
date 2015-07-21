using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// 文字列を文字ごとに分解する
	/// </summary>
	class CharacterSplitter : Splitter
	{
		public CharacterSplitter(Filter filter)
			: base(filter)
		{
		}

		public override IEnumerable<string> Split(string str)
		{
			var sr = new StringReader(str);
			var c = sr.Read();
			while (c >= 0)
			{
				var s = _filter.FilterElement(((char) c).ToString());
				if (s != null)
					yield return s;
				c = sr.Read();
			}
		}
	}
}
