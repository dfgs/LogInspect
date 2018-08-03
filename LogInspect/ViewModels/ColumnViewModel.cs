using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogLib;

namespace LogInspect.ViewModels
{
	public abstract class ColumnViewModel : ViewModel
	{
		public event EventHandler WidthChanged;

		public string Name
		{
			get;
			private set;
		}

		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ColumnViewModel),new PropertyMetadata(100d,WidthPropertyChanged));
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}


		public HorizontalAlignment Alignment
		{
			get;
			private set;
		}

		public ColumnViewModel(ILogger Logger,string Name,string Alignment) : base(Logger)
		{
			HorizontalAlignment alignment;
			this.Name = Name;
			if (Enum.TryParse<HorizontalAlignment>(Alignment, out alignment)) this.Alignment = alignment;
			else this.Alignment = HorizontalAlignment.Left;
		}


		private static void WidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ColumnViewModel)d).OnWidthChanged();
		}
		protected virtual void OnWidthChanged()
		{
			WidthChanged?.Invoke(this, EventArgs.Empty);
		}

		public abstract void RenderEvent(DrawingContext DrawingContext,Rect Rect,EventViewModel Event);

	}
}
