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
	/// <summary>
	///  Track a list of unique log value per column. Ex: Maintain list of severities without duplicate
	/// </summary>
	public class FilterItemSourcesViewModel:ViewModel
	{
		private string[] columns;
		private PropertyCollection<List<string>> items;
	
		public IEnumerable<string> this[string Column]
		{
			get { return items[Column]; }
		}

		public FilterItemSourcesViewModel(ILogger Logger , IEnumerable<Column> Columns) : base(Logger)
		{
			AssertParameterNotNull("Columns", Columns);

			items = new PropertyCollection<List<string>>();
			columns = Columns.Where(item => item.IsFilterItemSource).Select(item => item.Name).ToArray();
			foreach (string column in columns)
			{
				items[column]= new List<string>();
			}
					   	
		}

		

		public async Task Load(IEnumerable<Event> Items)
		{
			List<string> values;
			string value;

			await Task.Run(() =>
			{
				foreach (Event ev in Items)
				{
					foreach (string property in columns)
					{
						values = items[property];
						value = ev[property];//.GetEventValue(property);
						if (!values.Contains(value)) values.Add(value);
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
