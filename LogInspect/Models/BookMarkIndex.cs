using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public struct BookMarkIndex
	{
		
		public int LineIndex
		{
			get;
			private set;
		}
		public int EventIndex
		{
			get;
			private set;
		}
		
		public BookMarkIndex(int LineIndex,int EventIndex)
		{
			this.LineIndex = LineIndex;this.EventIndex = EventIndex;
		}

	}
}
