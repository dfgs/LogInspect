using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.ViewModels.Columns;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventViewModel : ViewModel
	{

		private Event ev;

		public IEnumerable<PropertyViewModel> Properties
		{
			get;
			private set;
		}

		public int EventIndex
		{
			get;
			private set;
		}

		public int LineIndex
		{
			get;
			private set;
		}

		public PropertyViewModel this[string PropertyName]
		{
			get { return Properties.FirstOrDefault(item=>item.Column.Name==PropertyName); }
		}
		

		public Brush Background
		{
			get;
			private set;
		}
		public Brush Foreground
		{
			get;
			private set;
		}
		public string RawLog
		{
			get { return ev.Log.ToSingleLine(); }
		}

		public Rule Rule
		{
			get { return ev.Rule; }
		}
		public string Lines
		{
			get
			{
				return string.Join("\r\n", ev.Log.Lines.Select(item=>item.Value));
			}
		}



		public static readonly DependencyProperty IsBookMarkedProperty = DependencyProperty.Register("IsBookMarked", typeof(bool), typeof(EventViewModel));
		public bool IsBookMarked
		{
			get { return (bool)GetValue(IsBookMarkedProperty); }
			set { SetValue(IsBookMarkedProperty, value); }
		}



		public EventViewModel(ILogger Logger, IEnumerable<ColumnViewModel> Columns,  IEnumerable<ColoringRule> ColoringRules, Event Event, int EventIndex, int LineIndex) : base(Logger)
		{
			//string severity;
			
			this.ev = Event;
			this.EventIndex = EventIndex;
			this.LineIndex = LineIndex;

			//public enum Severity {Debug,Info,Warning,Error,Critical};


			Background = Brushes.LightGray;
			Foreground = Brushes.Black;

			foreach (ColoringRule coloringRule in ColoringRules)
			{

				if (Regex.Match(Event.GetValue(coloringRule.Column) as string, coloringRule.Pattern).Success)
				{
					try
					{
						Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(coloringRule.Background));
						Foreground = Background;
						break;
					}
					catch(Exception ex)
					{
						Log(ex);
					}
					
				}
			}

			Properties = Columns.Select(item => item.CreatePropertyViewModel(this)).ToArray();

		}

		public object GetPropertyValue(string PropertyName)
		{
			return ev.GetValue(PropertyName);
		}

	}

}
