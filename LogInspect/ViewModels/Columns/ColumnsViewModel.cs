using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class ColumnsViewModel : ViewModel,IEnumerable<ColumnViewModel>
	{
		private List<ColumnViewModel> items;



		public ColumnsViewModel(ILogger Logger,FormatHandler FormatHandler, FilterItemSourcesViewModel FilterItemSourcesViewModel,IRegexBuilder RegexBuilder,IInlineColoringRuleDictionary InlineColoringRuleDictionary) : base(Logger,-1)
		{
			IInlineParser inlineParser;
			InlineColoringRule inlineColoringRule;

			items = new List<ColumnViewModel>();

			AddColumn(new BookMarkColumnViewModel(Logger, "BookMarked") { Width = 30 });
			AddColumn(new LineColumnViewModel(Logger, "Line number") { Width = 50 });

			foreach (Column column in FormatHandler.Columns)
			{
				inlineParser = new InlineParser(RegexBuilder);
				foreach(string ruleName in column.InlineColoringRules)
				{
					try
					{
						inlineColoringRule = InlineColoringRuleDictionary.GetItem(FormatHandler.NameSpace, ruleName);
					}
					catch(Exception ex)
					{
						Log(LogLevels.Warning, ex.Message);
						continue;
					}
					try
					{
						inlineParser.Add(FormatHandler.NameSpace, inlineColoringRule);
					}
					catch(Exception ex)
					{
						Log(LogLevels.Warning, ex.Message);
						continue;
					}
				}

				if (column.Name == FormatHandler.TimeStampColumn)
				{
					AddColumn(new TimeStampColumnViewModel(Logger, column.Name, column.Alignment,column.Format) { Width = column.Width });
				}
				else if (column.Name == FormatHandler.SeverityColumn) AddColumn(new SeverityColumnViewModel(Logger, column.Name, column.Alignment,inlineParser, FilterItemSourcesViewModel) { Width = column.Width });
				else if (column.IsFilterItemSource) AddColumn(new MultiChoicesColumnViewModel(Logger, column.Name, column.Alignment,inlineParser, FilterItemSourcesViewModel) { Width = column.Width });
				else AddColumn(new TextPropertyColumnViewModel(Logger, column.Name, column.Alignment,inlineParser) { Width = column.Width });
			}
			
			
		}

		private void AddColumn(ColumnViewModel Column)
		{
			//Column.WidthChanged += Column_WidthChanged;
			//Column.FilterChanged += Column_FilterChanged;
			items.Add(Column);
		}

		

		


		public IEnumerator<ColumnViewModel> GetEnumerator()
		{
			foreach (ColumnViewModel item in items) yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
