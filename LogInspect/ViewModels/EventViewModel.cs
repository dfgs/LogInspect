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

		
		public object Severity
		{
			get { return ev.Severity; }
		}
		
		public string RawLog
		{
			get { return ev.Log.ToSingleLine(); }
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

		



		public EventViewModel(ILogger Logger, Event Event, int EventIndex, int LineIndex) : base(Logger)
		{
			//string severity;

			this.ev = Event;
			this.EventIndex = EventIndex;
			this.LineIndex = LineIndex;

			
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
