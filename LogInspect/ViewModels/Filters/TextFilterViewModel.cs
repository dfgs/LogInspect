using LogInspect.Models.Filters;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels.Filters
{
	public class TextFilterViewModel:FilterViewModel
	{

		public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(TextConditions), typeof(TextFilterViewModel));
		public TextConditions Condition
		{
			get { return (TextConditions)GetValue(ConditionProperty); }
			set { SetValue(ConditionProperty, value); }
		}


		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TextFilter));
		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public TextFilterViewModel(ILogger Logger, string PropertyName, TextFilter Model):base(Logger,PropertyName)
		{
			if (Model!=null)
			{
				this.Value = Model.Value;
			}
		}

		public override Filter CreateFilter()
		{
			return new TextFilter(PropertyName) { Value=Value,Condition=Condition };
		}



	}
}
