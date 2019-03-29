using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public class FileLineReader : ILineReader
	{
		private StreamReader reader;
		public bool EOF => reader.BaseStream.Position==reader.BaseStream.Length;

		/*public long Length
		{
			get => reader.BaseStream.Length;
		}
		public long Position
		{
			get => reader.BaseStream.Position;
		}*/

		public FileLineReader(Stream Stream)
		{
			if (Stream == null) throw new ArgumentNullException("Stream");
			reader = new StreamReader(Stream);

		}


		public string Read()
		{
			return reader.ReadLine();
		}
	}
}
