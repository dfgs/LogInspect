using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public struct Event
	{
		public Log Log
		{
			get;
			private set;
		}

		public Rule Rule
		{
			get;
			private set;
		}

		public string Severity
		{
			get;
			private set;
		}

		public bool HasValidTimeStamp
		{
			get;
			private set;
		}

		public DateTime TimeStamp
		{
			get;
			private set;
		}

		public long Position
		{
			get { return Log.Position; }
		}

		public Property[] Properties
		{
			get;
			private set;
		}

		

		public Event(Log Log,Rule Rule, params Property[] Properties)
		{
			if (Properties == null) throw new ArgumentNullException("Properties");
			this.Log = Log;this.Rule = Rule;this.Properties = Properties;
			Severity = null;
			TimeStamp = DateTime.MinValue;
			HasValidTimeStamp = false;

			if (this.Rule.SeverityToken != null) Severity = GetValue(Rule.SeverityToken);
			if ((this.Rule.TimeStampToken!=null) && ((this.Rule.TimeStampFormat != null)))
			{
				DateTime result;
				HasValidTimeStamp=DateTime.TryParseExact(GetValue(Rule.TimeStampToken),Rule.TimeStampFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
				if (HasValidTimeStamp) TimeStamp = result;
			}
		}

		public Property GetProperty(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name);
		}
			
		public string GetValue(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name)?.Value;
		}

	}
}
