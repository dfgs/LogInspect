﻿using LogInspect.Models;
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
			AssertParameterNotNull("ColoringRules", ColoringRules);
			this.coloringRules = ColoringRules;
		}

		public string GetBackground( Event Event)
		{
			string value;

			foreach (EventColoringRule coloringRule in coloringRules)
			{
				value = Event[coloringRule.Column];
				if (value == null) continue;

				if (Regex.Match(value, coloringRule.Pattern).Success) return coloringRule.Background;
				/*{
					try
					{
						//Brush Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString());
						//Background.Freeze();	// mandatory for UI binding and avoid thread synchronisation exceptions
						return Background;
					}
					catch
					{
						Log(LogLevels.Error, $"Invalid background {coloringRule.Background}");
					}

				}*/
			}
			return "Transparent";
		}
		/*public Brush GetBackground(EventViewModel Event)
		{
			string value;

			foreach (EventColoringRule coloringRule in coloringRules)
			{
				value = Event[coloringRule.Column].Value?.ToString();
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
						Log(LogLevels.Error, $"Invalid background {coloringRule.Background}");
					}

				}
			}
			return null;
		}*/


	}
}
