using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class ColumnsViewModel : CollectionViewModel<Column,ColumnViewModel>
	{

		private FormatHandler formatHandler;
		private FilterItemSourcesViewModel filterItemSourcesViewModel;
		private IColorProviderModule colorProviderModule;
		private IInlineParserBuilderModule inlineParserBuilderModule;


		public ColumnsViewModel(ILogger Logger,FormatHandler FormatHandler, FilterItemSourcesViewModel FilterItemSourcesViewModel, IInlineParserBuilderModule InlineParserBuilderModule, IColorProviderModule ColorProviderModule) : base(Logger)
		{
			AssertParameterNotNull(FormatHandler,"FormatHandler", out formatHandler);
			AssertParameterNotNull(FilterItemSourcesViewModel,"FilterItemSourcesViewModel", out filterItemSourcesViewModel);
			AssertParameterNotNull(ColorProviderModule,"ColorProviderModule", out colorProviderModule);
			AssertParameterNotNull(InlineParserBuilderModule,"InlineParserBuilderModule", out inlineParserBuilderModule);

		
		}

		protected override IEnumerable<ColumnViewModel> GenerateItems(IEnumerable<Column> Items)
		{

			yield return new BookMarkColumnViewModel(Logger, "BookMarked") { Width = 30 };
			yield return new LineColumnViewModel(Logger, "Line number") { Width = 50 };

			foreach (Column column in Items)
			{

				if (column.Name == formatHandler.TimeStampColumn) yield return new TimeStampColumnViewModel(Logger, column.Name, column.Alignment, column.Format) { Width = column.Width };
				else if (column.Name == formatHandler.SeverityColumn) yield return new SeverityColumnViewModel(Logger, column.Name, column.Alignment,  filterItemSourcesViewModel,colorProviderModule) { Width = column.Width };
				else if (column.IsFilterItemSource) yield return new MultiChoicesColumnViewModel(Logger, column.Name, column.Alignment, filterItemSourcesViewModel) { Width = column.Width };
				else yield return new InlinePropertyColumnViewModel(Logger, column.Name, column.Alignment, inlineParserBuilderModule.CreateParser(formatHandler.NameSpace, column) ) { Width = column.Width };
			}


		}








	}
}
