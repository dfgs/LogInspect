using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public abstract class ColumnViewModel : ViewModel
	{
		public event EventHandler WidthChanged;
		public event EventHandler FilterChanged;

		public TextAlignment Alignment
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public abstract bool AllowsFilter
		{
			get;
		}
		public abstract bool AllowsResize
		{
			get;
		}

		private Filter filter;
		public Filter Filter
		{
			get { return filter; }
			set
			{
				if (filter == value) return;
				filter = value;
				FilterChanged?.Invoke(this, EventArgs.Empty);
				OnPropertyChanged("HasFilter");
			}
		}

		public bool HasFilter
		{
			get { return Filter != null; }
		}

		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ColumnViewModel),new PropertyMetadata(100d,WidthPropertyChanged));
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}
		



		public ColumnViewModel(ILogger Logger,string Name, string Alignment) : base(Logger)
		{
			TextAlignment alignment;


			if (Enum.TryParse<TextAlignment>(Alignment, out alignment)) this.Alignment = alignment;
			else this.Alignment = TextAlignment.Left;

			this.Name = Name;
		}


		private static void WidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ColumnViewModel)d).OnWidthChanged();
		}
		protected virtual void OnWidthChanged()
		{
			WidthChanged?.Invoke(this, EventArgs.Empty);
		}

		public abstract PropertyViewModel CreatePropertyViewModel(EventViewModel Event);
		public abstract FilterViewModel CreateFilterViewModel();

		
	}
}
