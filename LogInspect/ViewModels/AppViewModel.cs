using LogInspect.Models;
using LogInspectLib;
using LogInspectLib.Parsers;
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

		public void Open(LogFile LogFile,IRegexBuilder RegexBuilder,IInlineColoringRuleDictionary InlineColoringRuleDictionary)
		{
			LogFileViewModel logFile;

			try
			{
				logFile = new LogFileViewModel(Logger,LogFile,RegexBuilder, InlineColoringRuleDictionary);
				logFile.Load();
			}
			catch(Exception ex)
			{
				Log(ex);
				return;
			}
			Add(logFile);
			SelectedItem = logFile;
		}

		public void CloseCurrent()
		{
			if (SelectedItem == null) return;
			Remove(SelectedItem);
		}

		
	}
}
