using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;

namespace LogInspect.BaseLib.Builders
{
	public class LogBuilder : ILogBuilder
	{
		private IStringMatcher discardMatcher;
		private IStringMatcher appendLineToPreviousMatcher;
		private IStringMatcher appendLineToNextMatcher;

		private List<Line> buffer;

		public bool CanFlush => buffer.Count > 0;

		public LogBuilder(IStringMatcher DiscardMatcher, IStringMatcher AppendLineToPreviousMatcher, IStringMatcher AppendLineToNextMatcher)
		{
			if (DiscardMatcher == null) throw new ArgumentNullException("DiscardMatcher");
			if (AppendLineToPreviousMatcher == null) throw new ArgumentNullException("AppendLineToPreviousMatcher");
			if (AppendLineToNextMatcher == null) throw new ArgumentNullException("AppendLineToNextMatcher");

			this.discardMatcher = DiscardMatcher;
			this.appendLineToNextMatcher = AppendLineToNextMatcher;
			this.appendLineToPreviousMatcher = AppendLineToPreviousMatcher;

			buffer = new List<Line>();
		}

		public bool Push(Line Input, out Log Output)
		{
			bool previousLineMustAppendToNext;

			Output = null;
			if (discardMatcher.Match(Input.Value)) return false;

			
			if (buffer.Count>0)	previousLineMustAppendToNext = appendLineToNextMatcher.Match(buffer[buffer.Count - 1].Value);
			else previousLineMustAppendToNext = false;

			if (appendLineToPreviousMatcher.Match(Input.Value) || previousLineMustAppendToNext)
			{
				buffer.Add(Input);
				return false;
			}

			if (buffer.Count > 0)
			{
				Output = new Log();
				Output.Lines.AddRange(buffer);

				buffer.Clear();
				buffer.Add(Input);

				return true;
			}

			buffer.Add(Input);
			return false;


		}

		public Log Flush()
		{
			Log Output;

			if (buffer.Count == 0) throw new InvalidOperationException();

			Output = new Log();
			Output.Lines.AddRange(buffer);
			buffer.Clear();

			return Output;
		}

	}
}
