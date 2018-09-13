using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
	public interface IReader<T>
	{
		
		bool EndOfStream
		{
			get;
		}
		
		long Position
		{
			get;
		}

		long Length
		{
			get;
		}


		T Read();
		void Seek(long Position);
		



	}
}
