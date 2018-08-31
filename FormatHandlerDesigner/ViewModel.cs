using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FormatHandlerDesigner
{
	public class ViewModel:DependencyObject
	{

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ViewModel),new PropertyMetadata(null,ValuePropertyChanged));
		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public event EventHandler ValueChanged;

		public ViewModel()
		{

		}

		public ViewModel(object Value)
		{
			this.Value = Value;
		}


		private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ViewModel)d).OnValueChanged();
		}
		protected virtual void OnValueChanged()
		{
			ValueChanged?.Invoke(this, EventArgs.Empty);
		}



	}
}
