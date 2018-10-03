using LogInspect.ViewModels.Columns;
using LogInspectLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace LogInspect.Views
{
	public static class Inlines
	{
		private static BrushConverter converter = new BrushConverter();

		public static readonly DependencyProperty InlineSourceProperty = DependencyProperty.RegisterAttached("InlineSource", typeof(IEnumerable<Inline>), typeof(Inlines), new UIPropertyMetadata(null, InlineSourceChanged));

		[AttachedPropertyBrowsableForType(typeof(TextBlock))]
		public static IEnumerable<Inline> GetInlineSource(DependencyObject obj)
		{
			return (IEnumerable<Inline>)obj.GetValue(InlineSourceProperty);
		}

		public static void SetInlineSource(DependencyObject obj, IEnumerable<Inline> value)
		{
			obj.SetValue(InlineSourceProperty, value);
		}




		private static void InlineSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TextBlock textBlock;
			IEnumerable<Inline> inlines;
			System.Windows.Documents.Run run;

			textBlock = sender as TextBlock;
			if (textBlock == null) return;

			textBlock.Inlines.Clear();
			inlines = e.NewValue as IEnumerable<Inline>;
			if (inlines == null) return;

			foreach(Inline inline in inlines)
			{
				run = new System.Windows.Documents.Run();
				run.Text = inline.Value;
				try
				{
					run.Foreground = (Brush)converter.ConvertFromString(inline.Foreground);
				}
				catch
				{
					run.Foreground = Brushes.Black;
				}
				if (inline.Underline) run.TextDecorations=TextDecorations.Underline;
				if (inline.Bold) run.FontWeight=FontWeights.Bold;
				if (inline.Italic) run.FontStyle=FontStyles.Italic;
				textBlock.Inlines.Add(run);
			}
		}

		


		

		


		

	}
}
