using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LogInspectLib
{
    public struct Log
    {
        public Line[] Lines
        {
            get;
            private set;
        }
		/*public int Index
		{
			get;
			private set;
		}*/
		public long Position
		{
			get { return Lines[0].Position; }
		}

        
        public Log(params Line[] Lines)
        {
			//if (Index < 0) throw (new ArgumentException("Index"));
			if (Lines == null)  throw (new ArgumentNullException("Lines"));
			this.Lines = Lines;
        }

		public string ToSingleLine()
		{
			return String.Join("", Lines.Select(item=>item.Value));
		}
		public string ToMultipleLines()
		{
			return String.Join("\r\n", Lines.Select(item => item.Value));
		}
		public override string ToString()
		{
			return ToSingleLine();
		}
	}
}
