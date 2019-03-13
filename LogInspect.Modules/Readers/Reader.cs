using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Readers
{
	public abstract class Reader<T> : Module, IReader<T>
		where T:class
	{
		public abstract bool CanRead
		{
			get;
		}

		public Reader(ILogger Logger):base(Logger)
		{

		}

		protected abstract T OnRead();

		public T Read()
		{
			T result=null;
			result = OnRead();
			return result;
		}

	}
}
