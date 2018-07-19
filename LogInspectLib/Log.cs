using System;
using System.Collections.Generic;
using System.Text;

namespace LogInspectLib
{
    public struct Log
    {
        public Line[] Lines
        {
            get;
            private set;
        }
		public long Position
		{
			get { return Lines[0].Position; }
		}

        
        public Log(params Line[] Lines)
        {
			if ((Lines == null) || (Lines.Length == 0)) throw (new ArgumentNullException("Lines"));
			this.Lines = Lines;
        }

		public string ToSingleLine()
		{
			return String.Join("", Lines);
		}
		public string ToMultipleLines()
		{
			return String.Join("\r\n", Lines);
		}
		public override string ToString()
		{
			return ToSingleLine();
		}
	}
}
