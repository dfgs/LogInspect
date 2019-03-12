using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public abstract class LibraryModule<T> : Module, ILibraryModule<T>
	{
		protected LibraryModule(ILogger Logger) : base(Logger)
		{
		}

		protected abstract T OnLoadFile(string FileName);
		protected abstract void OnItemLoaded(T Item);

		public void LoadDirectory(string Path)
		{
			T item;

			Log(LogLevels.Information, $"Parsing directory {Path}");
			try
			{
				foreach (string FileName in Directory.EnumerateFiles(Path, "*.xml").OrderBy((fileName) => fileName))
				{
					Log(LogLevels.Information, $"Loading file {FileName}");
					if (!Try(() => OnLoadFile(FileName)).OrAlert(out item, "Failed to load file")) continue;
					Try(() => OnItemLoaded(item)).OrAlert("Failed to add item in library");
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}
		



	}
}
