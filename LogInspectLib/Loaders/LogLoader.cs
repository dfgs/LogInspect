﻿using LogInspectLib.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Loaders
{
	public class LogLoader:Loader<Log>,ILogLoader
	{
		private ILineReader lineReader;
		private IStringMatcher appendLineToPreviousMatcher;
		private IStringMatcher appendLineToNextMatcher;
		private Line line;

		public LogLoader(ILineReader LineReader, IStringMatcher AppendLineToPreviousMatcher, IStringMatcher AppendLineToNextMatcher)
		{
			if (LineReader == null) throw new ArgumentNullException("LineReader");
			if (AppendLineToPreviousMatcher == null) throw new ArgumentNullException("AppendLineToPreviousMatcher");
			if (AppendLineToNextMatcher == null) throw new ArgumentNullException("AppendLineToNextMatcher");
			this.lineReader = LineReader;
			this.appendLineToPreviousMatcher = AppendLineToPreviousMatcher;
			this.appendLineToNextMatcher = AppendLineToNextMatcher;
		}

		private Line PeekLine()
		{
			if (line == null) line=lineReader.Read();
			return line;
		}
		private Line PopLine()
		{
			Line result;
			result = PeekLine();
			line = null;
			return result;
		}

		protected override Log OnLoad()
		{
			Log log;
			Line item;


			log = new Log();

			do
			{
				item = PopLine();
				log.Lines.Add(item);
			} while (appendLineToNextMatcher.Match(item.Value));

			// we assume that lines are written atomically (even multiline logs)
			while (lineReader.CanRead) 
			{
				item = PeekLine();
				if (!appendLineToPreviousMatcher.Match(item.Value)) break;
				log.Lines.Add(PopLine());
				
			}

			return log;
		

		}

	}
}
