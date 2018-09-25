using LogInspect.Models;
using LogInspect.Modules;
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
		private int position;

		private IEventListModule eventList;

		private string[] columns;
		private Dictionary<string, List<string>> items;

		public IEnumerable<string> this[string Column]
		{
			get { return items[Column]; }
		}

		public FilterItemSourcesViewModel(ILogger Logger , int RefreshInterval,IEventListModule EventList, IEnumerable<Column> Columns) : base(Logger,RefreshInterval)
		{
			items = new Dictionary<string, List<string>>();
			columns = Columns.Where(item => item.IsFilterItemSource).Select(item => item.Name).ToArray();
			foreach (string column in columns)
			{
				items.Add(column, new List<string>());
			}

			this.eventList = EventList;

			
		}

		protected override void OnRefresh()
		{
			List<string> values;
			string value;
			int count;

			count = eventList.Count;
			for (int t = position ; t < count; t++)
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
			position = count;

		}//*/

		public IEnumerable<object> GetFilterChoices(string Property)
		{
			return items[Property];
		}


	}
}
