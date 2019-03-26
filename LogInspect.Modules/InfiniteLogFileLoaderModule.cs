using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogInspect.Models;
using LogLib;
using ModuleLib;

namespace LogInspect.Modules
{
	public class InfiniteLogFileLoaderModule : Module, ILogFileLoaderModule
	{
	

		public InfiniteLogFileLoaderModule(ILogger Logger) : base(Logger)
		{
		}

		public IEnumerable<Event> Load()
		{
			while(true)
			{
				Thread.Sleep(5000);
				yield return new Event();
			}
		}


	}
}
