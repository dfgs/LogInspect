using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Builders
{
	public interface IBuilder<TIn,TOut>
	{
		bool CanFlush
		{
			get;
		}

		bool Push(TIn Input,out TOut Output);
		TOut Flush();
	}
}
