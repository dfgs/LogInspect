using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using LogLib;

namespace LogInspect.Modules
{
	public class InlineFormatLibraryModule : LibraryModule<InlineFormatCollection>, IInlineFormatLibraryModule
	{
		private NameSpaceDictionary<InlineFormat> dictionary;
		
		public int Count
		{
			get { return dictionary.Count; }
		}

		public InlineFormatLibraryModule(ILogger Logger, IDirectoryEnumerator DirectoryEnumerator, IFileLoader<InlineFormatCollection> FileLoader) : base(Logger,DirectoryEnumerator,FileLoader)
		{
			dictionary = new NameSpaceDictionary<InlineFormat>();
		}

		public InlineFormat GetItem(string Name)
		{
			InlineFormat inlineFormat;
			if (!Try(() => dictionary.GetItem(Name)).OrWarn(out inlineFormat, (ex) => ex.Message)) return null;
			return inlineFormat;
		}

		public InlineFormat GetItem(string DefaultNameSpace, string Name)
		{
			InlineFormat inlineFormat;
			if (!Try(() => dictionary.GetItem(DefaultNameSpace,Name)).OrWarn(out inlineFormat, (ex) => ex.Message)) return null;
			return inlineFormat;
		}

		protected override void OnItemLoaded(InlineFormatCollection Item)
		{
			foreach(InlineFormat format in Item.Items)
			{
				dictionary.Add(Item.NameSpace, format.Name, format);
			}
		}



		
	}
}
