using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	public enum SplitType
	{
        // 文字単位
		Character,
        // 行単位
		Line
	}

    /// <summary>
    /// 比較する単位で分割する
    /// </summary>
	public abstract class Splitter
	{
		public static Splitter CreateSplitter(SplitType type, Filter filter)
		{
			switch (type)
			{
			case SplitType.Character:
				return new CharacterSplitter(filter);
			case SplitType.Line:
				return new LineSplitter(filter);
			}
			return null;
		}

		public Splitter(Filter filter)
		{
			_filter = filter;
		}

		public abstract IEnumerable<string> Split(string str);

		protected Filter _filter = null;
	}
}
