using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Parsers
{
	public class InlineParser: IInlineParser
	{
		private static Comparer<int> comparer=Comparer<int>.Default;

		private List<(InlineFormat InlineColoringRule, Regex Regex)> items;
		private IRegexBuilder regexBuilder;
		
		public InlineParser(IRegexBuilder RegexBuilder)
		{
			if (RegexBuilder == null) throw new ArgumentNullException("RegexBuilder");

			this.regexBuilder = RegexBuilder;
			items = new List<(InlineFormat InlineColoringRule, Regex Regex)>();
		}

		public void Add(string NameSpace,InlineFormat InlineColoringRule)
		{
			Regex regex;
			if (NameSpace == null) throw new Exception( "Inline coloring rule must be defined in a valid namespace");
			if (InlineColoringRule == null) throw new Exception("Inline coloring rule must be defined in a valid namespace");
			regex = regexBuilder.Build(NameSpace, InlineColoringRule.Pattern,InlineColoringRule.IgnoreCase);
			items.Add((InlineColoringRule,regex));
		}

		public IEnumerable<Inline> Parse(string Value)
		{
			Match match;
			int index;
			List<Inline> inlines;
			Inline inline,newInline,existing;

			
			if (Value == null) yield break;

			inlines = new List<Inline>();
			foreach ((InlineFormat InlineColoringRule, Regex Regex) item in items)
			{
				match = item.Regex.Match(Value);
				while (match.Success)
				{
					newInline=new Inline() { Index = match.Index, Length = match.Length, Foreground = item.InlineColoringRule.Foreground, Underline = item.InlineColoringRule.Underline, Bold = item.InlineColoringRule.Bold, Italic = item.InlineColoringRule.Italic, Value = match.Value,DocumentType=item.InlineColoringRule.DocumentType };
					existing = inlines.FirstOrDefault(i => i.Intersect(newInline));// in order to priorize matching rules
					if (existing == null) inlines.Add(newInline);
					/*else
					{
						int t = 0;
					}*/
					match = match.NextMatch();
				}
			}
			inlines.Sort((item1, item2) => comparer.Compare(item1.Index,item2.Index) );


			index = 0;
			foreach(Inline matchedInline in inlines)
			{
				//if (matchedInline.Index < index) continue;	// several coloring rules can apply to one property
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
