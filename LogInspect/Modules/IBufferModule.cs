using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IBufferModule<T>:IBaseModule
	{
		
		T this[int Index]
		{
			get;
		}

		IEnumerable<T> GetBuffer();
	}
}
