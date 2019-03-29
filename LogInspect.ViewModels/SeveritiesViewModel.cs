using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LogLib;

namespace LogInspect.ViewModels
{
	public class SeveritiesViewModel : CollectionViewModel<EventViewModel, string>
	{
		private string severityColumn;

		public SeveritiesViewModel(ILogger Logger,  string SeverityColumn) : base(Logger)
		{
			AssertParameterNotNull(SeverityColumn,"SeverityColumn", out severityColumn);
		}
		protected override string[] GenerateItems(IEnumerable<EventViewModel> Items)
		{
			return Items.Select((item) => item[severityColumn].Value.ToString()).Distinct().ToArray();
		}
	



	}
}
