using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public ColumnsViewModel(ILogger Logger,Rule Rule) : base(Logger)
		{
			string timeStampColumnName=null;

			items = new List<ColumnViewModel>();

			AddColumn(new BookMarkColumnViewModel(Logger, " ") { Width = 20 });
			AddColumn(new LineColumnViewModel(Logger, "#") { Width = 50 });
			if (Rule!=null) 
			{
				if ((Rule.TimeStampToken != null) && (Rule.TimeStampFormat != null))
				{
					AddColumn(new TimeStampColumnViewModel(Logger, "Date") { Width = 150 });
					timeStampColumnName = Rule.TimeStampToken;
				}
				foreach (Token property in Rule.Tokens.Where(item => (item.Name != null) && (item.Name!= timeStampColumnName) ))
				{
					AddColumn(new TextPropertyColumnViewModel(Logger, property.Name, property.Alignment) { Width = property.Width });
				}
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
