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
		private IInlineParserFactoryModule inlineParserBuilderModule;


		public ColumnsViewModel(ILogger Logger,FormatHandler FormatHandler, FilterItemSourcesViewModel FilterItemSourcesViewModel, IInlineParserFactoryModule InlineParserBuilderModule, IColorProviderModule ColorProviderModule) : base(Logger)
		{
			AssertParameterNotNull(FormatHandler,"FormatHandler", out formatHandler);
			AssertParameterNotNull(FilterItemSourcesViewModel,"FilterItemSourcesViewModel", out filterItemSourcesViewModel);
			AssertParameterNotNull(ColorProviderModule,"ColorProviderModule", out colorProviderModule);
			AssertParameterNotNull(InlineParserBuilderModule,"InlineParserBuilderModule", out inlineParserBuilderModule);

		
		}

		protected override ColumnViewModel[] GenerateItems(IEnumerable<Column> Items)
		{
			List<ColumnViewModel> items;

			items = new List<ColumnViewModel>();

			items.Add(new BookMarkColumnViewModel(Logger, "BookMarked") { Width = 30 });
			items.Add(new LineColumnViewModel(Logger, "Line number") { Width = 50 });

			foreach (Column column in Items)
			{

				if (column.Name == formatHandler.TimeStampColumn) items.Add(new TimeStampColumnViewModel(Logger, column.Name, column.Alignment, column.Format) { Width = column.Width });
				else if (column.Name == formatHandler.SeverityColumn) items.Add(new SeverityColumnViewModel(Logger, column.Name, column.Alignment,  filterItemSourcesViewModel,colorProviderModule) { Width = column.Width });
				else if (column.IsFilterItemSource) items.Add(new MultiChoicesColumnViewModel(Logger, column.Name, column.Alignment, filterItemSourcesViewModel) { Width = column.Width });
				else items.Add(new InlinePropertyColumnViewModel(Logger, column.Name, column.Alignment, inlineParserBuilderModule.CreateParser(formatHandler.NameSpace, column) ) { Width = column.Width });
			}

			return items.ToArray();
		}








	}
}
