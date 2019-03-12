using LogInspect.ViewModels.Pages;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public class PagesViewModel : CollectionViewModel<int, PageViewModel>
	{
		public PagesViewModel(ILogger Logger) : base(Logger)
		{
		}

		protected override IEnumerable<PageViewModel> GenerateItems(IEnumerable<int> Items)
		{
			throw new NotImplementedException();
		}
	}
}
