using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public class Page
	{
		public int Index
		{
			get;
			private set;
		}
		
		public int Size
		{
			get;
			private set;
		}

		private EventViewModel[] items;
		public EventViewModel this[int Index]
		{
			get { return items[Index]; }
			set { items[Index] = value; }
		}

		public bool IsComplete
		{
			get;
			set;
		}

		public Page(int Index,int Size)
		{
			this.Index = Index;
			this.Size = Size;
			items = new EventViewModel[Size];
			IsComplete = false;
		}


	}
}
