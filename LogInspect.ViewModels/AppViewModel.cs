using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.Models.Parsers;
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class AppViewModel : CollectionViewModel<LogFile,LogFileViewModel>
	{

		
		
		public AppViewModel(ILogger Logger) : base(Logger)
		{
		}

		protected override IEnumerable<LogFileViewModel> GenerateItems(IEnumerable<LogFile> Items)
		{
			throw new NotImplementedException();
		}

		public async Task Open(LogFile LogFile, IInlineParserBuilderModule InlineParserBuilderModule, IColorProviderModule ColorProviderModule)
		{
			LogFileViewModel logFile;

			AssertParameterNotNull("LogFile", LogFile);
			AssertParameterNotNull("InlineParserBuilderModule", InlineParserBuilderModule);
			AssertParameterNotNull("ColorProviderModule", ColorProviderModule);

			try
			{
				logFile = new LogFileViewModel(Logger,LogFile,InlineParserBuilderModule,ColorProviderModule);
				Add(logFile);
				SelectedItem = logFile;
				//await Task.Delay(5000);
				await logFile.Load();
			}
			catch (Exception ex)
			{
				Log(ex);
				return;
			}
		}

		public void Close(LogFileViewModel LogFile)
		{
			Remove(LogFile);
		}

		
	}
}
