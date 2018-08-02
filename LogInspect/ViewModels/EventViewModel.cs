using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public object Date
		{
			get;
			private set;
		}
		public object Severity
		{
			get;
			private set;
		}
		public object Thread
		{
			get;
			private set;
		}

		public object Message
		{
			get;
			private set;
		}

		public Rule Rule
		{
			get { return ev.Rule; }
		}
		public IEnumerable<string> Lines
		{
			get
			{
				return ev.Log.Lines.Select(item=>item.Value);
			}
		}

		public Brush Background
		{
			get;
			private set;
		}



		public EventViewModel(ILogger Logger, Event Event, int EventIndex, int LineIndex) : base(Logger)
		{
			string severity;

			this.ev = Event;
			this.EventIndex = EventIndex;
			this.LineIndex = LineIndex;
			this.Date = ev.GetProperty("Date")?.Value;
			this.Severity = ev.GetProperty("Severity")?.Value;
			this.Thread = ev.GetProperty("Thread")?.Value;
			if (ev.Rule == null) this.Message = ev.Log.ToSingleLine();
			else this.Message = ev.GetProperty("Message")?.Value;

			if (Severity!=null)
			{
				severity = Severity.ToString().ToUpper();
				if (severity.Contains("WARN")) Background = Brushes.Orange;
				if (severity.Contains("ERR")) Background = Brushes.OrangeRed;
				if (severity.Contains("FAT")) Background = Brushes.Red;
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
