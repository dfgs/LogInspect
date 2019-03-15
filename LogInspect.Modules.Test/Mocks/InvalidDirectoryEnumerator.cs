using LogInspect.BaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class InvalidDirectoryEnumerator : IDirectoryEnumerator
	{
		private int count;

		public InvalidDirectoryEnumerator(int Count)
		{
			this.count = Count;
		}

		public IEnumerable<string> EnumerateFiles(string Path)
		{
			for (int t = 0; t < count-1; t++) yield return $"Path{t}";
			throw (new Exception("Failed to enumerate last item"));
		}
	}
}
