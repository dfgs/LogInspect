using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class RateMeter
	{
		private int counter;
		private long startTicks;
		private int interval;

		public int MaxRate
		{
			get;
			private set;
		}

		public int Rate
		{
			get;
			private set;
		}

		public RateMeter(int Interval)
		{
			this.interval = Interval;
		}

		public void Start()
		{
			startTicks = Environment.TickCount;
			counter = 0;
			Rate = 0;
			MaxRate = 0;
		}

		public void Refresh(int Count)
		{
			long ticks;
			double seconds;

			counter+=Count;

			ticks = Environment.TickCount;
			seconds = TimeSpan.FromTicks(ticks - startTicks).TotalSeconds;
			if (seconds>interval)
			{
				Rate = (int)(counter / seconds);
				if (Rate > MaxRate) MaxRate = Rate;
				counter = 0;
				startTicks = ticks;
			}
		}




	}
}
