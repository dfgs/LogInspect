using LogInspect.BaseLib.Builders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLineBuilder : ILineBuilder
	{
		public bool CanFlush => false;

		public Line Flush()
		{
			throw new NotImplementedException();
		}

		public bool Push(string Input, out Line Output)
		{
			Output = new Line();
			Output.Value = Input;

			return true;
		}


	}
}
