using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Modules;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class ColumnsViewModel : ViewModel,IEnumerable<ColumnViewModel>
	{
		private List<ColumnViewModel> items;

		public event EventHandler FilterChanged;
		
		public double TotalWidth
		{
			get;
			private set;
		}

		public ColumnsViewModel(ILogger Logger,FormatHandler FormatHandler, SelectionFiltersIndexerModule SelectionFiltersIndexerModule) : base(Logger)
		{

			items = new List<ColumnViewModel>();

			AddColumn(new BookMarkColumnViewModel(Logger, " ") { Width = 20 });
			AddColumn(new LineColumnViewModel(Logger, "#") { Width = 50 });

			foreach (Column column in FormatHandler.Columns)
			{
				if (column.Name == FormatHandler.TimeStampColumn )
				{
					AddColumn(new TimeStampColumnViewModel(Logger, column.Name,column.Alignment) { Width = column.Width });
				}
				else if (column.IsFilterItemSource) AddColumn(new SelectionPropertyColumnViewModel(Logger, column.Name, column.Alignment,SelectionFiltersIndexerModule) { Width = column.Width });
				else AddColumn(new TextPropertyColumnViewModel(Logger, column.Name, column.Alignment) { Width = column.Width });
			}
			
			TotalWidth = items.Sum(item => item.Width);
		}

		private void AddColumn(ColumnViewModel Column)
		{
			Column.WidthChanged += Column_WidthChanged;
			Column.FilterChanged += Column_FilterChanged;
			items.Add(Column);
		}

		private void Column_FilterChanged(object sender, EventArgs e)
		{
			FilterChanged?.Invoke(this, e);
		}

		private void Column_WidthChanged(object sender, EventArgs e)
		{
			TotalWidth= items.Sum(item => item.Width);
			OnPropertyChanged("TotalWidth");
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
