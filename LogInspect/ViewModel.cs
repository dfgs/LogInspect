using LogLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect
{
	public abstract class ViewModel:DependencyObject,INotifyPropertyChanged, IDisposable
	{
		private static int counter;

		public event PropertyChangedEventHandler PropertyChanged;

		public int ID
		{
			get;
			private set;
		}

		public ILogger Logger
		{
			get;
			private set;
		}
		public ViewModel(ILogger Logger)
		{
			this.ID = counter;counter++;
			this.Logger = Logger;
		}
		public virtual void Dispose()
		{

		}

		protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName=null)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
		}

		protected string CreateExceptionMessage(Exception ex, [CallerMemberName]string MethodName = null)
		{
			return $"An unexpected exception occured in {GetType().Name}:{MethodName} ({ex.Message})";
		}
		protected void LogEnter([CallerMemberName]string MethodName = null)
		{
			Logger.LogEnter(ID, GetType().Name, MethodName);
		}
		protected void LogLeave([CallerMemberName]string MethodName = null)
		{
			Logger.LogLeave(ID, GetType().Name, MethodName);
		}
		protected void Log(LogLevels Level, string Message, [CallerMemberName]string MethodName = null)
		{
			Logger.Log(ID, GetType().Name, MethodName, Level, Message);
		}
		protected void Log(Exception ex, [CallerMemberName]string MethodName = null)
		{
			Logger.Log(ID, GetType().Name, MethodName, LogLevels.Error, CreateExceptionMessage(ex, MethodName));
		}


	}
}
