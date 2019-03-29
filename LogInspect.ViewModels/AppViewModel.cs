using LogInspect.Models;
using LogInspect.Modules;
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

		protected override LogFileViewModel[] GenerateItems(IEnumerable<LogFile> Items)
		{
			throw new NotImplementedException();
		}

		public async Task Open(LogFile LogFile, IInlineParserFactoryModule InlineParserFactoryModule, IColorProviderModule ColorProviderModule)
		{
			LogFileViewModel logFileViewModel;

			AssertParameterNotNull(LogFile,"LogFile");
			AssertParameterNotNull(InlineParserFactoryModule, "InlineParserFactoryModule");
			AssertParameterNotNull(ColorProviderModule,"ColorProviderModule");

			try
			{
				logFileViewModel = new LogFileViewModel(Logger,LogFile,InlineParserFactoryModule,ColorProviderModule);
				Add(logFileViewModel);
				SelectedItem = logFileViewModel;
				//await Task.Delay(1);
				await logFileViewModel.Load();
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
