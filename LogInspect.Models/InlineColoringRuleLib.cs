using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspect.Models
{
	[Serializable]
	public class InlineColoringRuleLib
	{
		[XmlAttribute]
		public string NameSpace
		{
			get;
			set;
		}

		[XmlArray]
		public List<InlineColoringRule> Items
		{
			get;
			set;
		}

		public InlineColoringRuleLib()
		{
			Items = new List<InlineColoringRule>();
		}
		public void SaveToFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Create))
			{
				SaveToStream(stream);
			}
		}
		public void SaveToStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(InlineColoringRuleLib));
			serializer.Serialize(Stream, this);
		}

		public static InlineColoringRuleLib LoadFromFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				return LoadFromStream(stream);
			}
		}
		public static InlineColoringRuleLib LoadFromStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(InlineColoringRuleLib));
			return (InlineColoringRuleLib)serializer.Deserialize(Stream);
		}


	}
}
