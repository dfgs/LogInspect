using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace LogInspect.ViewModels
{
	public class MarkersViewModel:CollectionViewModel<EventViewModel, MarkerViewModel>
	{
		private string severityColumn;

		public MarkersViewModel(ILogger Logger ,string SeverityColumn) : base(Logger)
		{
			AssertParameterNotNull(SeverityColumn,"SeverityColumn",out severityColumn);

		}

		protected override MarkerViewModel[] GenerateItems(IEnumerable<EventViewModel> Items)
		{
			object severity;
			MarkerViewModel marker = null;
			int index = 0;
			List<MarkerViewModel> items;

			items = new List<MarkerViewModel>();
			foreach (EventViewModel item in Items)
			{
				if ((item.Brush != null)&&(item.Brush!="Transparent"))
				{
					severity = item[severityColumn].Value;
					if ((marker == null) || (!ValueType.Equals(severity , marker.Severity)) || (index != marker.Position + marker.Size))
					{
						marker = new MarkerViewModel(Logger);
						marker.Position = index;
						marker.Background =  item.Brush;
						marker.Severity = severity;
						items.Add(marker);
					}
					marker.Size++;
				}
				index++;
			}

			return items.ToArray();
		}
		
		

	}
}
