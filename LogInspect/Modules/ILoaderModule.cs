using LogInspectLib.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface ILoaderModule<TLoader,T>:IBaseModule
		where TLoader:ILoader<T>
	{
		
		TLoader Loader
		{
			get;
		}

		bool Start();
		bool Stop();

	}
}
