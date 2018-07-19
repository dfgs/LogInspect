using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
	public abstract class Reader<T>
	{

		public abstract bool EndOfStream
		{
			get;
		}
		public abstract long Position
		{
			get;
		}

		public abstract T Read();

		public abstract void Seek(long Position);



	}
}
