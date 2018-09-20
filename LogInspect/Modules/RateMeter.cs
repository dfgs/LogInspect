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
		private DateTime startTime;
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
			startTime = DateTime.Now;
			counter = 0;
			Rate = 0;
			MaxRate = 0;
		}

		public void Refresh(int Count)
		{
			DateTime now;
			double seconds;

			counter+=Count;

			now = DateTime.Now;
			seconds = (now-startTime).TotalSeconds;
			if (seconds>interval)
			{
				Rate = (int)(counter / seconds);
				if (Rate > MaxRate) MaxRate = Rate;
				counter = 0;
				startTime = now;
			}
		}




	}
}
