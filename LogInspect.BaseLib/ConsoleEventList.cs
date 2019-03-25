using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;

namespace LogInspect.BaseLib
{
	public class ConsoleEventList : IEventList
	{
		public void Add(Event Event)
		{
			Console.WriteLine(string.Join("	", Event.Properties));
		}
	}
}
