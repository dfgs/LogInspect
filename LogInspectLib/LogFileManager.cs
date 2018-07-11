using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class LogFileManager:BaseComponent
	{
		private List<FormatHandler> formatHandlers;

		public LogFileManager(ILogger Logger)
			:base(Logger)
		{
			formatHandlers = new List<FormatHandler>();
		}

		public void LoadSchemas(string Path)
		{
			FormatHandler schema;

			Log(LogLevels.Information, "Parsing format handlers directory...");
			try
			{
				foreach (string FileName in Directory.EnumerateFiles(Path, "*.xml"))
				{
					Log(LogLevels.Information,$"Loading file {FileName}");
					try
					{
						schema = FormatHandler.LoadFromFile(FileName);
					}
					catch(Exception ex)
					{
						Log(ex);
						continue;
					}
					formatHandlers.Add(schema);
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}

		public LogReader CreateLogReader(string FileName)
		{
			Stream stream;
			LogReader reader;
			FormatHandler formatHandler;
			string shortName;

			
			shortName = Path.GetFileName(FileName);
			Log(LogLevels.Information, $"Try to find a format handler for file {shortName}");
			formatHandler = formatHandlers.FirstOrDefault(item => item.MatchFileName(shortName));
			if (formatHandler==null)
			{
				Log(LogLevels.Warning, $"Format of log file {shortName} is unmanaged");
				formatHandler = new FormatHandler();	// create a default handler
			}
			else
			{
				Log(LogLevels.Information, $"Format handler {formatHandler.Name} found for log file {shortName}");
			}

			Log(LogLevels.Information, "Creating log reader...");
			try
			{
				stream = new FileStream(FileName, FileMode.Open);
				reader = new LogReader(stream, formatHandler);
			}
			catch (Exception ex)
			{
				Log(ex);
				return null;
			}

			return reader;			
		}



	}
}
