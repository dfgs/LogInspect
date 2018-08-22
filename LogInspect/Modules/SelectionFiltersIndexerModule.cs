using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogInspect.Models;
using LogInspectLib;
using LogLib;
using ModuleLib;

namespace LogInspect.Modules
{
	public class SelectionFiltersIndexerModule : Module
	{
		private EventIndexerModule indexerModule;

		private string[] properties;
		private Dictionary<string, List<object>> items;
		public event IndexedEventHandler<string,object> Indexed;


		public SelectionFiltersIndexerModule(ILogger Logger, EventIndexerModule IndexerModule,FormatHandler FormatHandler) : base("SelectionFiltersIndexer", Logger)
		{
			items = new Dictionary<string, List<object>>();
			properties = FormatHandler.Columns.Where(item=>item.IsFilterItemSource).Select(item=>item.Name).ToArray();
			foreach (string Property in properties)
			{
				items.Add(Property, new List<object>());
			}
			this.indexerModule = IndexerModule;
			indexerModule.Read+= IndexerModule_Read;
		}
		public override void Dispose()
		{
			items = null;properties = null;
		}
		private void IndexerModule_Read(object sender, ReadEventArgs<Event> e)
		{
			List<object> values;
			object value;

			foreach (string Property in properties)
			{
				values = items[Property];
				value = e.Input.GetValue(Property);
				if (!values.Contains(value))
				{
					values.Add(value);
					Indexed?.Invoke(this, new IndexedEventArgs<string, object>(Property, value));
				}
			}
		}

		public IEnumerable<object> GetItemsSource(string Name)
		{
			return items[Name];
		}
		


		


	}
}
