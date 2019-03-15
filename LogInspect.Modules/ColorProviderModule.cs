using LogInspect.Models;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LogInspect.Modules
{
	public class ColorProviderModule : Module, IColorProviderModule
	{
		private IEnumerable<EventColoringRule> coloringRules;

		public ColorProviderModule(ILogger Logger, IEnumerable<EventColoringRule> ColoringRules) : base(Logger)
		{
			AssertParameterNotNull(ColoringRules,"ColoringRules", out coloringRules);
		}

		public string GetBackground( Event Event)
		{
			string value;

			if (!AssertParameterNotNull(Event, "Event")) return "Transparent";

			foreach (EventColoringRule coloringRule in coloringRules)
			{
				value = Event[coloringRule.Column];
				if (value == null) continue;

				if (Regex.Match(value, coloringRule.Pattern).Success) return coloringRule.Background;
				
			}
			return "Transparent";
		}


	}
}
