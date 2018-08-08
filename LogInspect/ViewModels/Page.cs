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
			set { items[Index] = value;LastFilledIndex = Index; }
		}

		public int LastFilledIndex
		{
			get;
			private set;
		}
		
		public bool IsComplete
		{
			get { return LastFilledIndex == Size - 1; }
		}

		public Page(int Index,int Size)
		{
			LastFilledIndex = -1;
			this.Index = Index;
			this.Size = Size;
			items = new EventViewModel[Size];
		}


	}
}
