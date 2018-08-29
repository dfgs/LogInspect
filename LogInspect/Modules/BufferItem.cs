using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
    public class BufferItem
    {
		public int EventIndex
		{
			get;
			set;
		}
		public int LineIndex
		{
			get;
			set;
		}
		public Event Event
		{
			get;
			set;
		}
    }
}
