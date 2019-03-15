using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Readers
{
	public abstract class Reader<T> :  IReader<T>
		where T:class
	{
		public abstract bool CanRead
		{
			get;
		}

		public Reader()
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
