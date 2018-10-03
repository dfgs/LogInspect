using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Parsers
{
	public class InlineParser:IInlineParser
	{
		private static Comparer<int> comparer=Comparer<int>.Default;

		private List<Tuple<InlineColoringRule,Regex>> items;
		private IRegexBuilder regexBuilder;
		
		public InlineParser(IRegexBuilder RegexBuilder)
		{
			if (RegexBuilder == null) throw new ArgumentNullException("RegexBuilder");
			items = new List<Tuple< InlineColoringRule, Regex>>();
			this.regexBuilder = RegexBuilder;
		}

		public void Add(string NameSpace,InlineColoringRule InlineColoringRule)
		{
			Regex regex;

			regex = regexBuilder.Build(NameSpace, InlineColoringRule.Pattern,InlineColoringRule.IgnoreCase);
			items.Add(new Tuple<InlineColoringRule, Regex>(InlineColoringRule,regex));
		}

		public IEnumerable<Inline> Parse(string Value)
		{
			Match match;
			int index;
			List<Inline> inlines;
			Inline inline;

			
			if (Value == null) yield break;

			inlines = new List<Inline>();
			foreach (Tuple<InlineColoringRule,Regex> item in items)
			{
				match = item.Item2.Match(Value);
				while (match.Success)
				{
					inlines.Add(new Inline() { Index = match.Index, Length = match.Length, Foreground = item.Item1.Foreground, Underline = item.Item1.Underline, Bold= item.Item1.Bold,Italic= item.Item1.Italic, Value = match.Value });
					match = match.NextMatch();
				}
			}
			inlines.Sort((item1, item2) => comparer.Compare(item1.Index,item2.Index) );


			index = 0;
			foreach(Inline matchedInline in inlines)
			{
				if (matchedInline.Index < index) continue;	// several coloring rules can apply to one property
				if (matchedInline.Index != index)
				{
					inline = new Inline();
					inline.Index = index;
					inline.Length = matchedInline.Index - index;
					inline.Foreground = "Black";


					inline.Value = Value.Substring(inline.Index, inline.Length);

					yield return inline;
					index += inline.Length;
				}
				yield return matchedInline;
				index += matchedInline.Length;

			}
			if (index<Value.Length)
			{
				inline = new Inline();
				inline.Index = index;
				inline.Length = Value.Length - index;
				inline.Foreground = "Black";
				inline.Value = Value.Substring(inline.Index, inline.Length);
				yield return inline;
			}

		}




	}



}
