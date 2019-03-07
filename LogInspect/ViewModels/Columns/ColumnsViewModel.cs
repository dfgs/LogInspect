using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class ColumnsViewModel : CollectionViewModel<Column,ColumnViewModel>
	{

		private FormatHandler formatHandler;
		private IRegexBuilder regexBuilder;
		private IInlineColoringRuleDictionary inlineColoringRuleDictionary;
		private FilterItemSourcesViewModel filterItemSourcesViewModel;

		public ColumnsViewModel(ILogger Logger,FormatHandler FormatHandler, FilterItemSourcesViewModel FilterItemSourcesViewModel,IRegexBuilder RegexBuilder,IInlineColoringRuleDictionary InlineColoringRuleDictionary) : base(Logger)
		{
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			AssertParameterNotNull("FormatHandler", FormatHandler);
			AssertParameterNotNull("InlineColoringRuleDictionary", InlineColoringRuleDictionary);
			AssertParameterNotNull("FilterItemSourcesViewModel", FilterItemSourcesViewModel);

			this.regexBuilder = RegexBuilder;
			this.formatHandler = FormatHandler;
			this.inlineColoringRuleDictionary = InlineColoringRuleDictionary;
			this.filterItemSourcesViewModel = FilterItemSourcesViewModel;
		}

		protected override IEnumerable<ColumnViewModel> GenerateItems(IEnumerable<Column> Items)
		{
			IInlineParser inlineParser;
			InlineColoringRule inlineColoringRule;

			yield return new BookMarkColumnViewModel(Logger, "BookMarked") { Width = 30 };
			yield return new LineColumnViewModel(Logger, "Line number") { Width = 50 };

			foreach (Column column in Items)
			{
				inlineParser = new InlineParser(regexBuilder);
				foreach (string ruleName in column.InlineColoringRules)
				{
					try
					{
						inlineColoringRule = inlineColoringRuleDictionary.GetItem(formatHandler.NameSpace, ruleName);
					}
					catch (Exception ex)
					{
						Log(LogLevels.Warning, ex.Message);
						continue;
					}
					try
					{
						inlineParser.Add(formatHandler.NameSpace, inlineColoringRule);
					}
					catch (Exception ex)
					{
						Log(LogLevels.Warning, ex.Message);
						continue;
					}
				}

				if (column.Name == formatHandler.TimeStampColumn)
				{
					yield return new TimeStampColumnViewModel(Logger, column.Name, column.Alignment, column.Format) { Width = column.Width };
				}
				else if (column.Name == formatHandler.SeverityColumn) yield return new SeverityColumnViewModel(Logger, column.Name, column.Alignment,  filterItemSourcesViewModel) { Width = column.Width };
				else if (column.IsFilterItemSource) yield return new MultiChoicesColumnViewModel(Logger, column.Name, column.Alignment, filterItemSourcesViewModel) { Width = column.Width };
				else yield return new InlinePropertyColumnViewModel(Logger, column.Name, column.Alignment, inlineParser) { Width = column.Width };
			}


		}








	}
}
