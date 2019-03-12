using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using LogInspect.ViewModels.Columns;
using LogInspect.Models;
using LogInspect.Models.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class InlinePropertyViewModel : PropertyViewModel
	{
		
		public List<XmlDocument> Documents
		{
			get;
			private set;
		}

		public InlinePropertyViewModel(ILogger Logger, string Name,  IEnumerable<Inline> Inlines) : base(Logger, Name,Inlines)
		{
			Documents = new List<XmlDocument>();
			foreach(Inline inline in Inlines.Where(item=>item.DocumentType==DocumentTypes.Xml))
			{
				XmlDocument document;
				document = new XmlDocument();
				try
				{
					document.LoadXml(inline.Value);
				}
				catch/*(Exception ex)//*/
				{
					continue;
				}
				Documents.Add(document);
			}
		}

		public override string ToString()
		{
			IEnumerable<Inline> inlines;

			inlines=Value as IEnumerable<Inline>;

			return string.Join(" ", inlines.Select((item) => item.Value));
		}


	}
}
