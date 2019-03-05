using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface ILogFileLoaderModule:IThreadModule
	{
		long Position
		{
			get;
		}
		long Length
		{
			get;
		}

		int Count
		{
			get;
		}

	}
}
