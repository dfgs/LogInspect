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
			get { return Properties.FirstOrDefault(item=>item.Name==PropertyName); }
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



		public EventViewModel(ILogger Logger, IEnumerable<ColumnViewModel> Columns,  IEnumerable<EventColoringRule> ColoringRules, Event Event, int EventIndex, int LineIndex) : base(Logger)
		{
			//string severity;
			
			this.ev = Event;
			this.EventIndex = EventIndex;
			this.LineIndex = LineIndex;

			//public enum Severity {Debug,Info,Warning,Error,Critical};


			Background = GetBackground(ColoringRules,Event );
			if (Background == null)
			{
				Background = Brushes.LightGray;
				Foreground = Brushes.Black;
			}
			else Foreground = Background;


			Properties = Columns.Select(item => item.CreatePropertyViewModel(this)).ToArray();

		}

		public static Brush GetBackground(IEnumerable<EventColoringRule> ColoringRules,Event Event)
		{
			string value;

			foreach (EventColoringRule coloringRule in ColoringRules)
			{
				value = Event.GetValue(coloringRule.Column) as string;
				if (value == null) continue;

				if (Regex.Match(value, coloringRule.Pattern).Success)
				{
					try
					{
						Brush Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(coloringRule.Background));
						return Background;
					}
					catch 
					{
						
					}

				}
			}
			return null;
		}

		public Inline[] GetPropertyInlines(string PropertyName)
		{
			return ev.GetProperty(PropertyName)?.Inlines;
		}
		public object GetPropertyValue(string PropertyName)
		{
			return ev.GetValue(PropertyName);
		}

	}

}
