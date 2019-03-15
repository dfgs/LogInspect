using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class FormatHandlerLibraryModule : LibraryModule<FormatHandler>, IFormatHandlerLibraryModule
	{
		private List<FormatHandler> items;
		private IRegexBuilder regexBuilder;

		public FormatHandlerLibraryModule(ILogger Logger,IDirectoryEnumerator DirectoryEnumerator, IFileLoader<FormatHandler> FileLoader, IRegexBuilder RegexBuilder) : base(Logger,DirectoryEnumerator,FileLoader)
		{
			AssertParameterNotNull(RegexBuilder,"RegexBuilder", out regexBuilder);
			items = new List<FormatHandler>();
		}

		
		protected override void OnItemLoaded(FormatHandler Item)
		{
			items.Add(Item);
		}

		private bool MatchFileName(string FileName, string FileNamePattern, string DefaultNameSpace)
		{
			Regex regex;

			try
			{
				regex = regexBuilder.Build(DefaultNameSpace, FileNamePattern, false);
				return regex.Match(FileName).Success;
			}
			catch (Exception ex)
			{
				Log(ex);
				return false;
			}
		}

		public FormatHandler GetFormatHandler(string FileName)
		{
			FormatHandler formatHandler;
			string shortName;


			shortName = Path.GetFileName(FileName);
			Log(LogLevels.Information, $"Try to find a format handler for file {shortName}");
			formatHandler = items.FirstOrDefault(item => MatchFileName(shortName, item.FileNamePattern, item.NameSpace));
			if (formatHandler == null)
			{
				Log(LogLevels.Warning, $"Format of log file {shortName} is unmanaged");
				formatHandler = new FormatHandler() {Name="Default handler" };    // create a default handler
			}
			else
			{
				Log(LogLevels.Information, $"Format handler {formatHandler.Name} found for log file {shortName}");
			}

			return formatHandler;
		}



	}
}
