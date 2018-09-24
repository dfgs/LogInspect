using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
	public abstract class Reader<T>:IReader<T>
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
			return OnRead();
		}

	}
}
