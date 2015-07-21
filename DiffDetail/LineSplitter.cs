using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// 文字列を行ごとに分解する
	/// </summary>
	class LineSplitter : Splitter
	{
		public LineSplitter(Filter filter)
			: base(filter)
		{
		}

		public override IEnumerable<string> Split(string str)
		{
			var sr = new StringReader(str);
			var line = sr.ReadLine();
			while (line != null)
			{
				var s = _filter.FilterElement(line);
				if (s != null)
					yield return s;
				line = sr.ReadLine();
			}
		}
	}
}
