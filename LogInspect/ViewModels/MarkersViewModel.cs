using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LogInspect.ViewModels
{
	public class MarkersViewModel:CollectionViewModel<EventViewModel, MarkerViewModel>
	{
		private string severityColumn;
		private IColorProviderModule colorProviderModule;

		public MarkersViewModel(ILogger Logger , IColorProviderModule ColorProviderModule,string SeverityColumn) : base(Logger)
		{
			AssertParameterNotNull("SeverityColumn", SeverityColumn);
			AssertParameterNotNull("ColorProviderModule", ColorProviderModule);

			this.colorProviderModule = ColorProviderModule;
			this.severityColumn = SeverityColumn;
		}

		protected override IEnumerable<MarkerViewModel> GenerateItems(IEnumerable<EventViewModel> Items)
		{
			object severity;
			MarkerViewModel marker = null;
			int index = 0;
			List<MarkerViewModel> items;
			Brush background;

			items = new List<MarkerViewModel>();
			foreach (EventViewModel item in Items)
			{
				background= colorProviderModule.GetBackground(item);
				if (background != null)
				{
					severity = item[severityColumn].Value;
					if ((marker == null) || (!ValueType.Equals(severity , marker.Severity)) || (index != marker.Position + marker.Size))
					{
						marker = new MarkerViewModel(Logger);
						marker.Position = index;
						marker.Background =  background;
						marker.Background.Freeze(); // mandatory for UI binding and avoid thread synchronisation exceptions
						marker.Severity = severity;
						items.Add(marker);
					}
					marker.Size++;
				}
				index++;
			}

			return items;
		}
		
		

	}
}
