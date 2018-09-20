using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogInspectLib.Loaders;
using LogLib;

namespace LogInspect.ViewModels.Loaders
{
	public class LoaderViewModel : ViewModel,ILoaderViewModel
	{

		public static readonly DependencyProperty RateProperty = DependencyProperty.Register("Rate", typeof(int), typeof(LoaderViewModel));
		public int Rate
		{
			get { return (int)GetValue(RateProperty); }
			private set { SetValue(RateProperty, value); }
		}


		public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(LoaderViewModel));
		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			private set { SetValue(CountProperty, value); }
		}

		private IBaseModule module;

		public LoaderViewModel(ILogger Logger, int RefreshInterval, IBaseModule Module) : base(Logger, RefreshInterval)
		{
			this.module = Module;
		}

		protected override void OnRefresh()
		{
			this.Rate = module.Rate;
			this.Count = module.Count;
		}
	}
}
