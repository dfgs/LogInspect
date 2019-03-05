using LogInspect.Models;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogInspectLib.Readers;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	// Mocked class in order to test UI
	public class InfiniteLogFileLoaderModule : ThreadModule, ILogFileLoaderModule
	{

		public long Position
		{
			get;
			private set;
		}

		public long Length => 100;

		public int Count
		{
			get;
			private set;
		}

		public InfiniteLogFileLoaderModule(ILogger Logger) : base(Logger)
		{
			
		}


		protected override void ThreadLoop()
		{
			while((State==ModuleStates.Started) && (Position<Length))
			{
				Position++;
				Count++;
				Thread.Sleep(2000);
			}
		}


	}
}
