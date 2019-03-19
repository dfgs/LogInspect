using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels.Filters
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
					return (Value.ToLower() == this.Value.ToLower()) ;
				case TextConditions.Contains:
					return Value.ToLower().Contains(this.Value.ToLower());
				case TextConditions.StartsWith:
					return Value.ToLower().StartsWith(this.Value.ToLower());
				default:
					return false;
			}
		}
	}
}
