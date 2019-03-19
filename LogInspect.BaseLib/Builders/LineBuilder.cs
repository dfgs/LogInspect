using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Builders
{
	public class LineBuilder:ILineBuilder
	{
		
		private IStringMatcher discardMatcher;
		private int index;

		public bool CanFlush => false;

		public LineBuilder( IStringMatcher DiscardMatcher)
		{

			if (DiscardMatcher == null) throw new ArgumentNullException("DiscardMatcher");

			this.discardMatcher = DiscardMatcher;
		}

		public bool Push(string Input, out Line Output)
		{
			if (discardMatcher.Match(Input))
			{
				index++;  
				Output = null;
				return false;
			}
			Output = new Line() { Value=Input,Index=index };
			index++;
			return true;
		}

		public Line Flush()
		{
			throw new InvalidOperationException();
		}

	}
}
