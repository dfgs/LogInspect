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

		private Column column;
		private Regex[] regexes;

		public InlineParser(Column Column, IRegexBuilder RegexBuilder, string DefaultNameSpace)
		{
			this.column = Column;
			regexes = new Regex[Column.InlineColoringRules.Count];
			for(int t=0;t<Column.InlineColoringRules.Count;t++)
			{
				regexes[t] = RegexBuilder.Build(DefaultNameSpace, Column.InlineColoringRules[t].Pattern);
			}
		}

		public IEnumerable<Inline> Parse(string Value)
		{
			Match match;
			int index;
			List<Inline> inlines;
			Inline inline;
			InlineColoringRule coloringRule;

			if (Value == null) yield break;

			inlines = new List<Inline>();
			for (int t = 0; t < column.InlineColoringRules.Count; t++)
			{
				match = regexes[t].Match(Value);
				coloringRule = column.InlineColoringRules[t];
				while (match.Success)
				{
					inlines.Add(new Inline() { Index = match.Index, Length = match.Length, Foreground = coloringRule.Foreground, Underline = coloringRule.Underline, Bold= coloringRule.Bold,Italic= coloringRule.Italic, Value = match.Value });
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
