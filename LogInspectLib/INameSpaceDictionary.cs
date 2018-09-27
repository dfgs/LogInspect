using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public interface INameSpaceDictionary<T>
	{
		void Add(string NameSpace, string Name, T Item);
		T GetItem(string Name);
		T GetItem(string DefaultNameSpace, string Name);
	}
}
