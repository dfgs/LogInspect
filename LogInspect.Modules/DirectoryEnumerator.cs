using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class DirectoryEnumerator : IDirectoryEnumerator
	{
		public IEnumerable<string> EnumerateFiles(string Path)
		{
			return Directory.EnumerateFiles(Path, "*.xml").OrderBy((fileName) => fileName);
		}
	}
}
