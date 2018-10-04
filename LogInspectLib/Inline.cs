using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class Inline
	{
		public int Index
		{
			get;
			set;
		}
		public int Length
		{
			get;
			set;
		}
		public string Value
		{
			get;
			set;
		}
		public string Foreground
		{
			get;
			set;
		}
		public bool Underline
		{
			get;
			set;
		}
		public bool Bold
		{
			get;
			set;
		}
		public bool Italic
		{
			get;
			set;
		}

		public override string ToString()
		{
			return $"{Index}: {Value}";
		}

		public bool Intersect(Inline Other)
		{
			return !((Index >= Other.Index + Other.Length) || (Index + Length <= Other.Index));
		}

	}
}
