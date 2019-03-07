using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public abstract class ColumnViewModel : ViewModel
	{
		public abstract Visibility ImageVisibility
		{
			get;
		}
		public abstract string ImageSource
		{
			get;
		}
			


		public TextAlignment Alignment
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
			TextAlignment alignment;

			if (Enum.TryParse<TextAlignment>(Alignment, out alignment)) this.Alignment = alignment;
			else this.Alignment = TextAlignment.Left;

			this.Name = Name;
			this.Description = Description;

		}

		

		public abstract PropertyViewModel CreatePropertyViewModel(Event Event);
		public abstract FilterViewModel CreateFilterViewModel();

		
	}
}
