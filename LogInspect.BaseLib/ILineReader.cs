﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public interface ILineReader
	{
		/*long Position
		{
			get;
		}
		long Length
		{
			get;
		}*/
		bool EOF
		{
			get;
		}
		string Read();
	}
}
