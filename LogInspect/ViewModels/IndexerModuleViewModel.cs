using LogInspect.Modules;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class IndexerModuleViewModel<TIndexerModule> : ViewModel
		where TIndexerModule : BaseEventModule
	{
		public TIndexerModule IndexerModule { get; }

		private Timer timer;


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(long), typeof(IndexerModuleViewModel<TIndexerModule>));
		public long Position
		{
			get { return (long)GetValue(PositionProperty); }
			private set { SetValue(PositionProperty, value); }
		}



		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(long), typeof(IndexerModuleViewModel<TIndexerModule>));
		public long Target
		{
			get { return (long)GetValue(TargetProperty); }
			private set { SetValue(TargetProperty, value); }
		}



		public static readonly DependencyProperty IndexedEventsCountProperty = DependencyProperty.Register("IndexedEventsCount", typeof(int), typeof(IndexerModuleViewModel<TIndexerModule>));
		public int IndexedEventsCount
		{
			get { return (int)GetValue(IndexedEventsCountProperty); }
			private set { SetValue(IndexedEventsCountProperty, value); }
		}


		public static readonly DependencyProperty RateProperty = DependencyProperty.Register("Rate", typeof(int), typeof(IndexerModuleViewModel<TIndexerModule>));
		public int Rate
		{
			get { return (int)GetValue(RateProperty); }
			private set { SetValue(RateProperty, value); }
		}



		private int refreshInterval;
		private int oldCount;

		//public event EventHandler Refreshed;

		public IndexerModuleViewModel(ILogger Logger, TIndexerModule IndexerModule,int RefreshInterval) : base(Logger)
		{
			oldCount = 0;this.refreshInterval = RefreshInterval;
			this.IndexerModule = IndexerModule;
			timer = new Timer(timerCallBack, null, 0, RefreshInterval);
		}
		public override void Dispose()
		{
			base.Dispose();
			timer.Dispose();

		}
		/*protected virtual void OnRefresh()
		{

		}*/
		private void timerCallBack(object state)
		{
			Dispatcher.Invoke(() => {
				Position = IndexerModule.Position;
				Target = IndexerModule.Target;
				IndexedEventsCount = IndexerModule.IndexedEventsCount;
				Rate = (int)((IndexedEventsCount - oldCount)*1000 / refreshInterval);
				oldCount = IndexedEventsCount;
			});
		}


	}
}
