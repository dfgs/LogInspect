using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LogLib;

namespace LogInspect.ViewModels.Pages
{
	public class XmlPageViewModel : PageViewModel
	{
		public override string ImageSource => "/LogInspect;component/Images/text_align_left.png";

		public override string Name => "Xml document";

		private XmlDocument document;
		public XmlDocument Document
		{
			get { return document; }
		}
		public XmlPageViewModel(ILogger Logger,XmlDocument Document) : base(Logger)
		{
			AssertParameterNotNull(Document,"Document", out document);
		}
	}
}
