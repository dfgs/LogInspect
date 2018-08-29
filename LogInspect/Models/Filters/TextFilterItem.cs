using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models.Filters
{
	public class TextFilterItem
	{
		public static Array Conditions = Enum.GetValues(typeof(TextConditions));

		public string Value
		{
			get;
			set;
		}

		public TextConditions Condition
		{
			get;
			set;
		}
		
		public bool Match(string Value)
		{
			if (Value == null) return false;

			switch (Condition)
			{
				case TextConditions.Equals:
					return (Value == this.Value) ;
				case TextConditions.Contains:
					return Value.Contains(this.Value);
				case TextConditions.StartsWith:
					return Value.StartsWith(this.Value);
				default:
					return false;
			}
		}
	}
}
