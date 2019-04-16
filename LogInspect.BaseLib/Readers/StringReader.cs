using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Readers
{
	public class StringReader:IStringReader
	{
		private StreamReader reader;

		public StringReader( StreamReader Reader)
		{
			if (Reader == null) throw new ArgumentNullException("Reader");
			this.reader = Reader;
		}

		public string Read()
		{
			return reader.ReadLine();
		}

	}
}
