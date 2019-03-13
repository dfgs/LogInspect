using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public abstract class ColumnViewModel : ViewModel
	{
		public abstract bool IsImageVisible
		{
			get;
		}
		public abstract string ImageSource
		{
			get;
		}
			


		public string Alignment
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public string Format
		{
			get;
			set;
		}
		public abstract bool AllowsFilter
		{
			get;
		}
		public abstract bool AllowsResize
		{
			get;
		}

		private Filter filter;
		public Filter Filter
		{
			get { return filter; }
			set
			{
				if (filter == value) return;
				filter = value;
				OnPropertyChanged("HasFilter");
			}
		}

		public bool HasFilter
		{
			get { return Filter != null; }
		}

		public double Width
		{
			get;
			set;
		}

		


		public ColumnViewModel(ILogger Logger, string Name, string Description, string Alignment) : base(Logger)
		{
			this.Alignment = Alignment;

			this.Name = Name;
			this.Description = Description;

		}

		public ColumnFormatViewModel CreateColumnFormatViewModel()
		{
			return new ColumnFormatViewModel(Logger,this) { Name=this.Name,Format=this.Format };
		}

		public abstract PropertyViewModel CreatePropertyViewModel(Event Event);
		public abstract FilterViewModel CreateFilterViewModel();

		
	}
}
