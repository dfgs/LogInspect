using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public abstract class ColumnViewModel : ViewModel
	{
		public event EventHandler WidthChanged;

		public string Name
		{
			get;
			private set;
		}

		public abstract bool AllowResize
		{
			get;
		}

		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ColumnViewModel),new PropertyMetadata(100d,WidthPropertyChanged));
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}


		

		public ColumnViewModel(ILogger Logger,string Name) : base(Logger)
		{

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

		//public abstract object GetValue(EventViewModel Event);

	}
}
