using LogInspect.ViewModels;
using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LogInspect.Views
{
	public class IndexerBindingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			EventViewModel ev;
			Property property;
			string name;

			ev = (EventViewModel)value;
			name = parameter as string;

			property = ev.GetProperty(name);
			return property?.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
