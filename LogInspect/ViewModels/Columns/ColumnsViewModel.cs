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

		
		public double TotalWidth
		{
			get;
			private set;
		}

		public ColumnsViewModel(ILogger Logger,Rule Rule) : base(Logger)
		{
			ColumnViewModel column;

			items = new List<ColumnViewModel>();

			column = new BookMarkColumnViewModel(Logger, " ") { Width = 20 };
			column.WidthChanged += Column_WidthChanged;
			items.Add(column);
			column = new LineColumnViewModel(Logger, "#") { Width = 50 };
			column.WidthChanged += Column_WidthChanged;
			items.Add(column);
			if (Rule!=null) 
			{
				foreach (Token property in Rule.Tokens.Where(item => item.Name != null))
				{
					column = new TextPropertyColumnViewModel(Logger, property.Name, property.Alignment) { Width = property.Width };
					column.WidthChanged += Column_WidthChanged;
					items.Add(column);
				}
			}
			TotalWidth = items.Sum(item => item.Width);
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
