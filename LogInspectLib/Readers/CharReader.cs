using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
	public class CharReader:Reader<char>
	{
		private Stream stream;
		private int bufferSize;
		private char[] buffer;
		private int bufferIndex;
		private Encoding encoding;

		private long bufferPosition;
		private long position;
		public override long Position
		{
			get { return position; }
		}
		public override long Length
		{
			get { return stream.Length; }
		}

		public override bool EndOfStream
		{
			get { return position == stream.Length; }
		}

		public CharReader(Stream Stream,Encoding Encoding,int BufferSize)
		{
			if (Stream==null) throw new ArgumentNullException("Stream"); 
			if (Encoding==null) throw new ArgumentNullException("Encoding");
			if (BufferSize <= 0) throw new ArgumentException("BufferSize");

			this.bufferSize = BufferSize;
			this.stream = Stream;
			this.encoding = Encoding;
			bufferIndex = 0;
			position = stream.Position;
		}

		private void Load()
		{
			int count;
			byte[] bytes;

			bufferPosition = position;

			bytes = new byte[bufferSize];
			count = stream.Read(bytes, 0, bufferSize);
			if (count <= 0) throw (new EndOfStreamException());
			else this.buffer = encoding.GetChars(bytes, 0, count);
			bufferIndex = 0;
		}
		private async Task LoadAsync()
		{
			int count;
			byte[] bytes;

			bufferPosition = position;

			bytes = new byte[bufferSize];
			count = await stream.ReadAsync(bytes, 0, bufferSize);
			if (count <= 0) throw (new EndOfStreamException());
			else this.buffer = encoding.GetChars(bytes, 0, count);
			bufferIndex = 0;
		}
		protected override char OnRead()
		{
			char c;

			if ((buffer==null) || (bufferIndex == buffer.Length)) Load();
			c = buffer[bufferIndex];
			position += encoding.GetByteCount(buffer, bufferIndex, 1);
			bufferIndex++;
			return c;
		}
		
		protected override void OnSeek(long Position)
		{
			if ((Position >= bufferPosition) && (Position < bufferPosition + bufferSize))
			{
				bufferIndex = (int)(Position - bufferPosition);
			}
			else
			{
				buffer = null;
				stream.Seek(Position, SeekOrigin.Begin);
			}
			position = Position;
		}
		



	}
}
