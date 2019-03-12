using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models.Readers
{
	public interface IReader<T>
		where T : class
	{
		bool CanRead
		{
			get;
		}
		T Read();
	}
}
