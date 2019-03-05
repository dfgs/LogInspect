using LogInspect.Models;

using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LogInspect.ViewModels
{
	public class FilterItemSourcesViewModel:ViewModel
	{
		private string[] columns;
		private PropertyCollection<List<string>> items;

		public IEnumerable<string> this[string Column]
		{
			get { return items[Column]; }
		}

		public FilterItemSourcesViewModel(ILogger Logger ,IEnumerable<Column> Columns) : base(Logger)
		{
			AssertParameterNotNull("Columns", Columns);

			items = new PropertyCollection<List<string>>();
			columns = Columns.Where(item => item.IsFilterItemSource).Select(item => item.Name).ToArray();
			foreach (string column in columns)
			{
				items[column]= new List<string>();
			}
					   	
		}

		/*protected override void OnRefresh()
		{
			List<string> values;
			string value;
			int target;

			if (eventList.Count - position > 100) target = position + 100;      // smooth list loading
			else target = eventList.Count;
			for (int t = position ; t < target; t++)
			{
				foreach (string property in columns)
				{
					values = items[property];
					value = eventList[t][property];//.GetEventValue(property);
					if (!values.Contains(value))
					{
						values.Add(value);
						//FilterChoiceAdded?.Invoke(this, new FilterItemAddedEventArgs(property, value));
					}
				}
			}
			position = target;

		}//*/

		public IEnumerable<object> GetFilterChoices(string Property)
		{
			return items[Property];
		}


	}
}
