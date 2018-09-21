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
		private string line;

		public override bool CanLoad
		{
			get { return true; }
		}

		protected override Log OnLoad()
		{
			Log result;
			result = new Log();
			
			if (line==null)	result.Lines.Add( new Line() { Position = count, Value = $"A{count} | B{count} | C{count}" });
			else result.Lines.Add(new Line() { Position = count, Value = line});
			count++;
			line = null;
			return result;
		}

		public void Load(string Line)
		{
			this.line = Line;
			Load();
		}

	}
}
