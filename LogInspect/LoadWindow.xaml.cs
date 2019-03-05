using LogInspect.Models;
using LogInspect.Modules;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace LogInspect
{
	/// <summary>
	/// Logique d'interaction pour LoadWindow.xaml
	/// </summary>
	public partial class LoadWindow : Window
	{
		private DispatcherTimer timer;
		private ILogFileLoaderModule logFileLoaderModule;
		private bool dialogResult;


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(long), typeof(LoadWindow), new PropertyMetadata(0L));
		public long Position
		{
			get { return (long)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}


		public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(long), typeof(LoadWindow),new PropertyMetadata(100L));
		public long Length
		{
			get { return (long)GetValue(LengthProperty); }
			set { SetValue(LengthProperty, value); }
		}


		public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(LoadWindow));
		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			set { SetValue(CountProperty, value); }
		}


		public LoadWindow(ILogFileLoaderModule LogFileLoaderModule)
		{
			InitializeComponent();
			if (LogFileLoaderModule == null) throw new ArgumentNullException("LogFileLoaderModule");
			this.logFileLoaderModule = LogFileLoaderModule;

			dialogResult = true;

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(500);
			timer.Tick += Timer_Tick;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = logFileLoaderModule.State == ModuleStates.Started;
		}


		private void Timer_Tick(object sender, EventArgs e)
		{
			if (logFileLoaderModule.State != ModuleStates.Started)
			{
				timer.Stop();
				DialogResult = dialogResult;
			}

			Length = logFileLoaderModule.Length;
			Position = logFileLoaderModule.Position;
			Count = logFileLoaderModule.Count;
		}

		public bool? Load()
		{
			logFileLoaderModule.Start();
			timer.Start();
			return this.ShowDialog();
		}
				

		private void ButtonStop_Click(object sender, RoutedEventArgs e)
		{
			dialogResult = true;
			logFileLoaderModule.Stop();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			dialogResult = false;
			logFileLoaderModule.Stop();
		}

	}
}
