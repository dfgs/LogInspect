using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public interface ILineReader
	{
		bool EOF
		{
			get;
		}
		string Read();
	}
}
