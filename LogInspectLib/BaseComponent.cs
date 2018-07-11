using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class BaseComponent
	{
		private ILogger logger;
		private static int idCounter = 0;
		private int counter;

		public BaseComponent(ILogger Logger)
		{
			this.counter = idCounter;
			idCounter++;
			this.logger = Logger;
		}
		public void Log(Exception ex,[CallerMemberName]string MethodName=null)
		{
			logger.Log(counter, GetType().Name, MethodName, ex);
		}
		public void Log(LogLevels Level,string Message, [CallerMemberName]string MethodName = null)
		{
			logger.Log(counter, GetType().Name, MethodName,Level,Message);
		}

	}
}
