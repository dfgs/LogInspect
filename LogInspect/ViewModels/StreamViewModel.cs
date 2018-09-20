using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogLib;

namespace LogInspect.ViewModels
{
	public class StreamViewModel : ViewModel
	{
		private Stream stream;


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(long), typeof(StreamViewModel));
		public long Position
		{
			get { return (long)GetValue(PositionProperty); }
			private set { SetValue(PositionProperty, value); }
		}


		public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(long), typeof(StreamViewModel));
		public long Length
		{
			get { return (long)GetValue(LengthProperty); }
			private set { SetValue(LengthProperty, value); }
		}

		public StreamViewModel(ILogger Logger, int RefreshInterval,Stream Stream) : base(Logger, RefreshInterval)
		{
			this.stream = Stream;
		}

		protected override void OnRefresh()
		{
			this.Position = stream.Position;
			this.Length = stream.Length;
		}

	}
}
