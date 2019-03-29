using LogInspect.BaseLib.Builders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLineBuilderWithFlush : ILineBuilder
	{
		private Line line=null;

		public bool CanFlush => line!=null;

		public Line Flush()
		{
			Line result;
			result = line;
			line = null;
			return result;
		}

		public bool Push(string Input, out Line Output)
		{
			Output = line;
			line = new Line();
			line.Value = Input;

			return Output!=null;
		}


	}
}
