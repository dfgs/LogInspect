using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Loaders
{
	public interface ILoader<T>
	{
		/*bool CanLoad
		{
			get;
		}*/
		int Count
		{
			get;
		}
		/*T this[int Index]
		{
			get;
		}*/


		IEnumerable<T> GetBuffer();
		void Load();
	}
}
