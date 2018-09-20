using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class ColumnsViewModel : ViewModel,IEnumerable<ColumnViewModel>
	{
		private List<ColumnViewModel> items;


		


		public ColumnsViewModel(ILogger Logger,FormatHandler FormatHandler, FilterItemSourcesViewModel FilterItemSourcesViewModel) : base(Logger,-1)
		{

			items = new List<ColumnViewModel>();

			AddColumn(new BookMarkColumnViewModel(Logger, "BookMarked") { Width = 20 });
			AddColumn(new LineColumnViewModel(Logger, "Line number") { Width = 50 });

			foreach (Column column in FormatHandler.Columns)
			{
				if (column.Name == FormatHandler.TimeStampColumn)
				{
					AddColumn(new TimeStampColumnViewModel(Logger, column.Name, column.Alignment,column.Format) { Width = column.Width });
				}
				else if (column.Name == FormatHandler.SeverityColumn) AddColumn(new SeverityColumnViewModel(Logger, column.Name, column.Alignment, FilterItemSourcesViewModel) { Width = column.Width });
				else if (column.IsFilterItemSource) AddColumn(new MultiChoicesColumnViewModel(Logger, column.Name, column.Alignment, FilterItemSourcesViewModel) { Width = column.Width });
				else AddColumn(new TextPropertyColumnViewModel(Logger, column.Name, column.Alignment) { Width = column.Width });
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
