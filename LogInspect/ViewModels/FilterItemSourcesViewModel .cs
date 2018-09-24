using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Loaders;
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

		private ILogLoader eventLoader;

		private string[] columns;
		private Dictionary<string, List<string>> items;

		public IEnumerable<string> this[string Column]
		{
			get { return items[Column]; }
		}

		public FilterItemSourcesViewModel(ILogger Logger , int RefreshInterval,ILogLoader EventLoader, IEnumerable<Column> Columns) : base(Logger,RefreshInterval)
		{
			items = new Dictionary<string, List<string>>();
			columns = Columns.Where(item => item.IsFilterItemSource).Select(item => item.Name).ToArray();
			foreach (string column in columns)
			{
				items.Add(column, new List<string>());
			}

			this.eventLoader = EventLoader;

			
		}

		/*protected override void OnRefresh()
		{
			List<string> values;
			string value;
			int count;

			count = eventLoader.Count;
			for (int t = position ; t < count; t++)
			{
				foreach (string property in columns)
				{
					values = items[property];
					value = eventLoader[t][property];
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
