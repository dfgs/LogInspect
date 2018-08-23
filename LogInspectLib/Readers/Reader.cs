using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
	public abstract class Reader<T>
	{
		
		public abstract bool EndOfStream
		{
			get;
		}
		/*public int Index
		{
			get;
			private set;
		}*/
		public abstract long Position
		{
			get;
		}
		public abstract long Length
		{
			get;
		}
		public Reader()
		{
		}

		public T Read()
		{
			T item = OnRead();
			return item;
		}
		public async Task<T> ReadAsync()
		{
			T item = await OnReadAsync();
			return item;
		}

		protected abstract T OnRead();
		protected abstract Task<T> OnReadAsync();

		protected abstract void OnSeek(long Position);
		

		public void Seek(long Position)
		{
			OnSeek(Position);
		}
		



	}
}
