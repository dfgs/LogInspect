using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventViewModel : ViewModel
	{

		private Event ev;


		public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(EventViewModel));
		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			private set { SetValue(IndexProperty, value); }
		}


		public EventViewModel(ILogger Logger,Event Event,int Index) : base(Logger)
		{
			this.ev = Event;this.Index = Index;
		}

		public Property GetProperty(string Name)
		{
			return ev.GetProperty(Name);
		}


	}
}
