using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Loaders
{
	public interface ILoader<T>
	{
		/*long Position
		{
			get;
		}

		long Length
		{
			get;
		}*/

		bool CanLoad
		{
			get;
		}
		int Count
		{
			get;
		}
		T this[int Index]
		{
			get;
		}

		void Load();
	}
}
