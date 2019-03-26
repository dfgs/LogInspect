using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public class StreamProgressReporter : IProgressReporter
	{
		private Stream stream;

		public long Position => stream.Position;

		public long Length => stream.Length;

		public StreamProgressReporter(Stream Stream)
		{
			if (Stream == null) throw new ArgumentNullException("Stream");
			this.stream = Stream;
		}


	}
}
