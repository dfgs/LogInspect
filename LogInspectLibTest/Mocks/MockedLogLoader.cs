using LogInspectLib;
using LogInspectLib.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLibTest.Mocks
{
	public class MockedLogLoader : Loader<Log>, ILogLoader
	{
		private int count = 0;


		protected override Log OnLoad()
		{
			Log result;
			result = new Log();
			
			result.Lines.Add( new Line() { Position = count, Value = $"A{count} | B{count} | C{count}" });
			count++;
			return result;
		}

		

	}
}
