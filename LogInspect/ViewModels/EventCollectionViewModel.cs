using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventCollectionViewModel : CollectionViewModel<EventViewModel>
	{
		public EventCollectionViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
