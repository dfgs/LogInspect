using LogInspect.Modules;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public class IndexerModuleViewModel<TIndexerModule,T> : ViewModel
		where TIndexerModule : BaseEventModule<T>
	{
		private TIndexerModule indexerModule;
		public TIndexerModule IndexerModule
		{
			get { return indexerModule; }
		}

		private Timer timer;

		private long position;
		public long Position
		{
			get { return position; }
			private set { position = value; OnPropertyChanged(); }
		}

		private long target;
		public long Target
		{
			get { return target; }
			private set { target = value;OnPropertyChanged(); }
		}

		private int indexedEventsCount;
		public int IndexedEventsCount
		{
			get { return indexedEventsCount; }
			private set { indexedEventsCount = value;OnPropertyChanged(); }
		}
		public IndexerModuleViewModel(ILogger Logger, TIndexerModule IndexerModule,int RefreshInterval) : base(Logger)
		{
			this.indexerModule = IndexerModule;
			timer = new Timer(timerCallBack, null, 0, RefreshInterval);
		}
		public override void Dispose()
		{
			base.Dispose();
			timer.Dispose();

		}
		protected virtual void OnRefresh()
		{

		}
		private void timerCallBack(object state)
		{
			Dispatcher.Invoke(() => {
				Position = indexerModule.Position;
				Target = indexerModule.Target;
				IndexedEventsCount = indexerModule.IndexedEventsCount;
				OnRefresh();
			});
		}


	}
}
