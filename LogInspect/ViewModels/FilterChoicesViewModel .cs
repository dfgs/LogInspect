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

namespace LogInspect.ViewModels
{
	public class FilterChoicesViewModel:BaseCollectionViewModel<EventViewModel>
	{

		private string[] properties;
		private Dictionary<string, List<object>> items;

		public event FilterChoiceAddedEventHandler FilterChoiceAdded;

		public FilterChoicesViewModel(ILogger Logger ,EventIndexerModule IndexerModule,IEnumerable<Column> Columns) : base(Logger)
		{
			items = new Dictionary<string, List<object>>();
			properties = Columns.Where(item => item.IsFilterItemSource).Select(item => item.Name).ToArray();
			foreach (string Property in properties)
			{
				items.Add(Property, new List<object>());
			}
			IndexerModule.Read += IndexerModule_Read;
		}

		

		private void IndexerModule_Read(object sender, EventReadEventArgs e)
		{
			List<object> values;
			object value;

			Dispatcher.Invoke(() =>
			{
				foreach (string property in properties)
				{
					values = items[property];
					value = e.Event.GetValue(property);
					if (!values.Contains(value))
					{
						values.Add(value);
						FilterChoiceAdded?.Invoke(this, new FilterChoiceAddedEventArgs(property, value));
					}
				}
			});
		}

		public IEnumerable<object> GetFilterChoices(string Property)
		{
			return items[Property];
		}


	}
}
