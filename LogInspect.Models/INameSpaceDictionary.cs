﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public interface INameSpaceDictionary<T>
	{
		int Count
		{
			get;
		}

		T GetItem(string Name);
		T GetItem(string DefaultNameSpace, string Name);
	}
}
