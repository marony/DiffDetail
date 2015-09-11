using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDetail
{
	/// <summary>
	/// 何もしないフィルタ
	/// </summary>
	public class NothingFilter : Filter
	{
		public override string FilterElement(string str)
		{
			return str;
		}
	}
}
