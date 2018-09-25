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
	public abstract class ModuleViewModel : ViewModel,IModuleViewModel
	{

		public static readonly DependencyProperty RateProperty = DependencyProperty.Register("Rate", typeof(int), typeof(ModuleViewModel));
		public int Rate
		{
			get { return (int)GetValue(RateProperty); }
			private set { SetValue(RateProperty, value); }
		}


		public static readonly DependencyProperty MaxRateProperty = DependencyProperty.Register("MaxRate", typeof(int), typeof(ModuleViewModel));
		public int MaxRate
		{
			get { return (int)GetValue(MaxRateProperty); }
			private set { SetValue(MaxRateProperty, value); }
		}



		public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(ModuleViewModel));
		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			private set { SetValue(CountProperty, value); }
		}


		public static readonly DependencyProperty ProceededCountProperty = DependencyProperty.Register("ProceededCount", typeof(int), typeof(ModuleViewModel));
		public int ProceededCount
		{
			get { return (int)GetValue(ProceededCountProperty); }
			set { SetValue(ProceededCountProperty, value); }
		}

		protected IBaseModule Module
		{
			get;
			private set;
		}

		public ModuleViewModel(ILogger Logger, int RefreshInterval, IBaseModule Module) : base(Logger, RefreshInterval)
		{
			this.Module = Module;
		}

		protected override void OnRefresh()
		{
			this.Rate = Module.Rate;
			this.MaxRate = Module.MaxRate;
			this.Count = Module.Count;
			this.ProceededCount = Module.ProceededCount;
		}
	}
}
