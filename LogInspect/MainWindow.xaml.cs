using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogInspect
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			/*
			 * FormatHandler schema = new FormatHandler();
			schema.Name = "RCM";
			schema.FileNamePattern = @"^RCM\.log(\.\d+)?$";
			schema.AppendToNextPatterns.Add(@".*(?<!\u0003)$");
			

			rule = new LogInspectLib.Rule() { Name = "Event" };
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"^\d\d/\d\d/\d\d \d\d:\d\d:\d\d\.\d+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^ ]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Thread", Pattern = @"[^,]+(, *[^,]+)*" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Module", Pattern = @"[^ ]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @"[^\u0003]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\u0003$" });
			schema.Rules.Add(rule);

			rule = new LogInspectLib.Rule() { Name = "Comment" };
			rule.Tokens.Add(new Token() { Pattern = @"^//" });
			schema.Rules.Add(rule);


			schema.SaveToFile(@"FormatHandlers\RCM.xml");

			schemaManager = new LogFileManager(logger);
			schemaManager.LoadSchemas("FormatHandlers");
			*/
		}
	}
}
