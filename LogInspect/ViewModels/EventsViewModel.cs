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

namespace LogInspect.ViewModels
{
	public class EventsViewModel:BaseCollectionViewModel<EventViewModel>
	{
		
		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<EventColoringRule> coloringRules;

		public static readonly DependencyProperty TailProperty = DependencyProperty.Register("Tail", typeof(bool), typeof(EventsViewModel));
		public bool Tail
		{
			get { return (bool)GetValue(TailProperty); }
			set { SetValue(TailProperty, value); }
		}


		public EventsViewModel(ILogger Logger , EventIndexerBufferModule BufferModule,IEnumerable<ColumnViewModel> Columns,IEnumerable<EventColoringRule> ColoringRules) : base(Logger)
		{
			BufferModule.EventsBuffered += BufferModule_EventsBuffered;
			BufferModule.Reseted += BufferModule_Reseted;
			this.columns = Columns;
			this.coloringRules = ColoringRules;
		}

		

		private void BufferModule_Reseted(object sender, EventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				Clear();
			});
		}
		private void BufferModule_EventsBuffered(object sender, EventsBufferedEventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				List<EventViewModel> list = new List<EventViewModel>();
				list.AddRange(e.Items.Select(item => new EventViewModel(Logger, columns, coloringRules, item.Event, item.EventIndex, item.LineIndex)));
				AddRange(list) ;
				if (Tail) Select(Count - 1);
			});
		}


	}
}
