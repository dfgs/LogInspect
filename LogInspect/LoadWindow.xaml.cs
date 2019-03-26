using LogInspect.BaseLib;
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
		private ILogFileLoaderModule logFileLoaderModule;
		private IProgressReporter progressReporter;
		private LogFile logFile;
		private BackgroundWorker worker;
		private bool terminated;
		private bool returnResults;


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


		public LoadWindow(ILogFileLoaderModule LogFileLoaderModule, IProgressReporter ProgressReporter,  LogFile LogFile)
		{
			InitializeComponent();
			if (LogFileLoaderModule == null) throw new ArgumentNullException("LogFileLoaderModule");
			if (ProgressReporter == null) throw new ArgumentNullException("ProgressReporter");
			if (LogFile == null) throw new ArgumentNullException("LogFile");


			this.logFileLoaderModule = LogFileLoaderModule;
			this.progressReporter = ProgressReporter;
			this.logFile = LogFile;

			terminated = false;

			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += Worker_DoWork;
			worker.ProgressChanged += Worker_ProgressChanged;
		}

		
		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = !terminated;
		}


		

		public bool? Load()
		{
			worker.RunWorkerAsync();
			return this.ShowDialog();
		}

		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.Count = logFile.Events.Count;
			this.Length = progressReporter.Length;
			this.Position = progressReporter.Position;
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			DateTime startTime,time;

			returnResults = true;
			startTime = DateTime.Now;
			
			foreach (Event ev in logFileLoaderModule.Load())
			{
				if (worker.CancellationPending) break;
				logFile.Events.Add(ev);
				time = DateTime.Now;
				if ((time-startTime).TotalMilliseconds>=500)
				{
					startTime = time;
					worker.ReportProgress(100);
				}
			}

			terminated = true;
			Dispatcher.Invoke(() => DialogResult = returnResults);
		}

		private void ButtonStop_Click(object sender, RoutedEventArgs e)
		{
			returnResults = true;
			worker.CancelAsync();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			returnResults = false;
			worker.CancelAsync();
		}

	}
}
