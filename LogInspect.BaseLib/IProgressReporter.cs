using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public interface IProgressReporter
	{
		long Position
		{
			get;
		}
		long Length
		{
			get;
		}
	}
}
