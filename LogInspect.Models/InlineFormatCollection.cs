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
	public class InlineFormatCollection
	{
		[XmlAttribute]
		public string NameSpace
		{
			get;
			set;
		}

		[XmlArray]
		public List<InlineFormat> Items
		{
			get;
			set;
		}

		public InlineFormatCollection()
		{
			Items = new List<InlineFormat>();
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

			serializer = new XmlSerializer(typeof(InlineFormatCollection));
			serializer.Serialize(Stream, this);
		}

		public static InlineFormatCollection LoadFromFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				return LoadFromStream(stream);
			}
		}
		public static InlineFormatCollection LoadFromStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(InlineFormatCollection));
			return (InlineFormatCollection)serializer.Deserialize(Stream);
		}


	}
}
