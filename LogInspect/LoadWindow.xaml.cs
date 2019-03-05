using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LogInspect
{
	/// <summary>
	/// Logique d'interaction pour LoadWindow.xaml
	/// </summary>
	public partial class LoadWindow : Window
	{
		private ILogFileLoaderModule logFileLoaderModule;
		public LoadWindow(ILogFileLoaderModule LogFileLoaderModule)
		{
			InitializeComponent();
			if (LogFileLoaderModule == null) throw new ArgumentNullException("LogFileLoaderModule");
			this.logFileLoaderModule = LogFileLoaderModule;
		}

		public bool? Load()
		{
			logFileLoaderModule.Start();
			return this.ShowDialog();
		}

		

		private void ButtonStop_Click(object sender, RoutedEventArgs e)
		{
			logFileLoaderModule.Stop();
			DialogResult = true;
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			logFileLoaderModule.Stop();
			DialogResult = false;
		}

	}
}
