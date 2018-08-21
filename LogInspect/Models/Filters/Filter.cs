using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public abstract class Filter
    {
		//public abstract Filter Clone();

		public abstract bool MustDiscard(FileIndex Item);
	}
}
