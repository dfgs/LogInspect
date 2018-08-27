using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public class TextFilter:Filter
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

		public TextFilter(string PropertyName):base(PropertyName)
		{
			
		}
		public override bool MustDiscard(Event Item)
		{
			string value;

			value = Item.GetValue(PropertyName)?.ToString();
			if (value == null) return true;
			switch (Condition)
			{
				case TextConditions.Equals:
					return value != Value;
				case TextConditions.Contains:
					return !value.Contains(Value);
				case TextConditions.StartsWith:
					return !value.StartsWith(Value);
				default:
					return false;
			}
		}

	}
}
