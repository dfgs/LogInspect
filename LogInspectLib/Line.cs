using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public struct Line
	{
		public long Position
		{
			get;
			private set;
		}
		public string Value
		{
			get;
			private set;
		}


		public Line(long Position,string Value)
		{
			if (Position < 0) throw new ArgumentException("Position");
			if (Value == null) throw new ArgumentNullException("Value");

			this.Position = Position;this.Value = Value;
		}
	}
}
