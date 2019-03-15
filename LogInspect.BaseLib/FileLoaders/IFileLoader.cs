using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.FileLoaders
{
	public interface IFileLoader<T>
	{
		T Load(string FileName);
	}
}
