using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Readers
{
	public interface IReader<T>:IModule
		where T : class
	{
		bool CanRead
		{
			get;
		}
		T Read();
	}
}
