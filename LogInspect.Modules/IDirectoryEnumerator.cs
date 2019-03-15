﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IDirectoryEnumerator
	{
		IEnumerable<string> EnumerateFiles(string Path);
	}
}
