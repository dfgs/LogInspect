using LogInspectLib;
using LogInspectLib.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLibTest.Mocks
{
	public class MockedLineLoader : Loader<Line>, ILineLoader
	{
		private int count = 0;
		private string val = null;


		protected override Line OnLoad()
		{
			Line result;
			if (val == null) result = new Line() { Position = count, Value = $"Item {count}" };
			else result = new Line() { Position = count, Value = val };
			val = null;
			count++;
			return result;
		}

		public void Load(string Value)
		{
			val = Value;Load();
		}

	}
}
