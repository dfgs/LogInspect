using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class Property
	{
		private static Property[] emptyProperties = new Property[0];
		public static Property[] EmptyProperties
		{
			get { return emptyProperties; }
		}

		public string Name
		{
			get;
			set;
		}
		public object Value
		{
			get;
			set;
		}

		public Inline[] Inlines
		{
			get;
			set;
		}

		public Property()
		{

		}

	}
}
