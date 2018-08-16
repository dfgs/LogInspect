using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventViewModel : ViewModel
	{

		private Event ev;


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

		
		public string Severity
		{
			get { return ev.Severity; }
		}
		
		public Brush Background
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





		public EventViewModel(ILogger Logger, IEnumerable<SeverityMapping> SeverityMapping, Event Event, int EventIndex, int LineIndex) : base(Logger)
		{
			//string severity;

			this.ev = Event;
			this.EventIndex = EventIndex;
			this.LineIndex = LineIndex;

			//public enum Severity {Debug,Info,Warning,Error,Critical};

			Background = Brushes.Transparent;
			foreach (SeverityMapping mapping in SeverityMapping)
			{
				if (Regex.Match(Event.Severity, mapping.Pattern).Success)
				{
					switch(mapping.Severity)
					{
						case "Warning":
							Background = Brushes.Orange;
							break;
						case "Error":
							Background = Brushes.OrangeRed;
							break;
						case "Critical":
							Background = Brushes.Red;
							break;
					}
					break;
				}
			}

		}

		public string GetPropertyValue(string PropertyName)
		{
			object value;
			value = ev.GetProperty(PropertyName)?.Value;
			if (value == null) return null;
			return value.ToString();
		}

	}

}
