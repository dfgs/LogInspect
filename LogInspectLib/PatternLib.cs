using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable,XmlRoot("PatternLib")]
	public class PatternLib:List<Pattern>
	{
		[XmlAttribute]
		public string NameSpace
		{
			get;
			set;
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

			serializer = new XmlSerializer(typeof(PatternLib));
			serializer.Serialize(Stream, this);
		}

		public static PatternLib LoadFromFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				return LoadFromStream(stream);
			}
		}
		public static PatternLib LoadFromStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(PatternLib));
			return (PatternLib)serializer.Deserialize(Stream);
		}
	}
}
