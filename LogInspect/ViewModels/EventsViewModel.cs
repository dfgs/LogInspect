using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LogInspect.ViewModels
{
	public class EventsViewModel:CollectionViewModel<EventViewModel>
	{
		private int position;

		private IEventLoader eventLoader;

		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<EventColoringRule> coloringRules;

		public static readonly DependencyProperty TailProperty = DependencyProperty.Register("Tail", typeof(bool), typeof(EventsViewModel));
		public bool Tail
		{
			get { return (bool)GetValue(TailProperty); }
			set { SetValue(TailProperty, value); }
		}

		private Filter[] filters;

		public EventsViewModel(ILogger Logger , int RefreshInterval, IEventLoader EventLoader,IEnumerable<ColumnViewModel> Columns,IEnumerable<EventColoringRule> ColoringRules) : base(Logger, RefreshInterval)
		{
			this.columns = Columns;
			this.coloringRules = ColoringRules;

			this.eventLoader = EventLoader;
			
		}
		protected override void OnRefresh()
		{
			int count;
			EventViewModel vm;

			lock (this)
			{
				List<EventViewModel> list = new List<EventViewModel>();
				count = eventLoader.Count;

				for (int t = position; t < count; t++)
				{
					vm = new EventViewModel(Logger, columns, coloringRules, eventLoader[t]);
					vm.EventIndex = t;
					if (MustDiscard(vm)) continue;
					list.Add(vm);
				}
				position = count;
				AddRange(list);
				if (Tail) Select(Count - 1);

			}
		}
		private bool MustDiscard(EventViewModel Event)
		{
			if (filters == null) return false;
			foreach(Filter filter in filters)
			{
				if (filter.MustDiscard(Event)) return true;
			}
			return false;
		}
		public void SetFilters(Filter[] Filters)
		{
			lock (this)
			{
				this.filters = Filters;
				this.position = 0;
				Reset();
			}
		}


	}
}
