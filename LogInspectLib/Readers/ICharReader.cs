using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
	public interface ICharReader:IReader<char>
	{
		long AvailableBytes
		{
			get;
		}
		/*Encoding Encoding
		{
			get;
		}*/


	}
}
