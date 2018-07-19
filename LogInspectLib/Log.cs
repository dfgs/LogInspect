using System;
using System.Collections.Generic;
using System.Text;

namespace LogInspectLib
{
    public class Log
    {
        public List<string> Lines
        {
            get;
            private set;
        }
		public long Position
		{
			get;
			set;
		}

        public Log()
        {
            this.Lines = new List<string>();
        }
        public Log(IEnumerable<string> Lines,long Position)
        {
			this.Lines = new List<string>(Lines);
			this.Position = Position;
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
