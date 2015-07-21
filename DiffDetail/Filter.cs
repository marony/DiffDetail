using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// 比較前に実行するフィルタ
	/// </summary>
	public abstract class Filter
	{
		public static Filter CreteFilter(string fileName)
		{
			if (fileName.ToUpper().EndsWith(@".CS"))
				return new CSharpFilter();

			return new NothingFilter();
		}

		public abstract string FilterElement(string str);
	}
}
