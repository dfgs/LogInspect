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
		private ILineLoader lineLoader;
		private IStringMatcher appendLineToPreviousMatcher;
		private IStringMatcher appendLineToNextMatcher;
		private int lineIndex;

		

		public LogLoader(ILineLoader LineLoader, IStringMatcher AppendLineToPreviousMatcher, IStringMatcher AppendLineToNextMatcher)
		{
			if (LineLoader == null) throw new ArgumentNullException("LineLoader");
			if (AppendLineToPreviousMatcher == null) throw new ArgumentNullException("AppendLineToPreviousMatcher");
			if (AppendLineToNextMatcher == null) throw new ArgumentNullException("AppendLineToNextMatcher");
			this.lineLoader = LineLoader;
			this.appendLineToPreviousMatcher = AppendLineToPreviousMatcher;
			this.appendLineToNextMatcher = AppendLineToNextMatcher;
		}

		protected override Log OnLoad()
		{
			Line line;
			Log log;
			int backupIndex;


			backupIndex = lineIndex;

			try
			{
				log = new Log();

				do
				{
					line = lineLoader[lineIndex];
					lineIndex++;
					log.Lines.Add(line);
				} while (appendLineToNextMatcher.Match(line.Value));

				while (lineIndex < lineLoader.Count)
				{
					line = lineLoader[lineIndex];
					if (!appendLineToPreviousMatcher.Match(line.Value)) break;

					log.Lines.Add(line);
					lineIndex++;

				}

				return log;
			}
			catch (Exception ex)
			{
				lineIndex = backupIndex;
				throw (ex);

			}

		}

	}
}
