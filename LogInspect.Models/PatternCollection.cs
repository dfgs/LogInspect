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
	public class PatternCollection
	{
		[XmlAttribute]
		public string NameSpace
		{
			get;
			set;
		}

		[XmlArray]
		public List<Pattern> Items
		{
			get;
			set;
		}

		public PatternCollection()
		{
			Items = new List<Pattern>();
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

			serializer = new XmlSerializer(typeof(PatternCollection));
			serializer.Serialize(Stream, this);
		}

		public static PatternCollection LoadFromFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				return LoadFromStream(stream);
			}
		}
		public static PatternCollection LoadFromStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(PatternCollection));
			return (PatternCollection)serializer.Deserialize(Stream);
		}


	}
}
