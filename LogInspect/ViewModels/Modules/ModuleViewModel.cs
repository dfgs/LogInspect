using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogLib;

namespace LogInspect.ViewModels.Modules
{
	public class ModuleViewModel : ViewModel,IModuleViewModel
	{

		public static readonly DependencyProperty RateProperty = DependencyProperty.Register("Rate", typeof(int), typeof(ModuleViewModel));
		public int Rate
		{
			get { return (int)GetValue(RateProperty); }
			private set { SetValue(RateProperty, value); }
		}


		public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(ModuleViewModel));
		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			private set { SetValue(CountProperty, value); }
		}

		private IBaseModule module;

		public ModuleViewModel(ILogger Logger, int RefreshInterval, IBaseModule Module) : base(Logger, RefreshInterval)
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
