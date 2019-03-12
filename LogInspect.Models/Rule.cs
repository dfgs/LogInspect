using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspect.Models
{
	[Serializable]
	public class Rule
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool Discard
		{
			get;
			set;
		}
		[XmlArray]
		public List<Token> Tokens
		{
			get;
			set;
		}

		public Rule()
		{
			Tokens = new List<Token>();
		}

		/*public IEnumerable<string> GetColumns()
		{
			return Tokens.Where(item => item.Name != null).Select(item => item.Name);
		}*/

		public string GetPattern()
		{
			return String.Join("", Tokens.Select(item => item.GetPattern()));
		}

	}


}
