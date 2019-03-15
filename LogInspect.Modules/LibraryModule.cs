using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
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
		private IDirectoryEnumerator directoryEnumerator;
		private IFileLoader<T> fileLoader;

		protected LibraryModule(ILogger Logger, IDirectoryEnumerator DirectoryEnumerator, IFileLoader<T> FileLoader) : base(Logger)
		{
			AssertParameterNotNull(DirectoryEnumerator, "DirectoryEnumerator",out directoryEnumerator);
			AssertParameterNotNull(FileLoader, "FileLoader", out fileLoader);
		}

		protected abstract void OnItemLoaded(T Item);

		public void LoadDirectory(string Path)
		{
			T item;

			if (!AssertParameterNotNull(Path, "Path")) return;

			Log(LogLevels.Information, $"Parsing directory {Path}");
			try
			{
				foreach (string FileName in directoryEnumerator.EnumerateFiles(Path))
				{
					Log(LogLevels.Information, $"Loading file {FileName}");
					if (!Try(() => fileLoader.Load(FileName)).OrAlert(out item, "Failed to load file")) continue;
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
