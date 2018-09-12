using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogInspect.Views
{
    public static class Commands
    {
		public static RoutedUICommand FindPrevious = new RoutedUICommand();
		public static RoutedUICommand FindNext = new RoutedUICommand();
		public static RoutedUICommand FindPreviousSeverity = new RoutedUICommand();
		public static RoutedUICommand FindNextSeverity = new RoutedUICommand();
		public static RoutedUICommand FindPreviousBookMark = new RoutedUICommand();
		public static RoutedUICommand FindNextBookMark = new RoutedUICommand();
		public static RoutedUICommand ToogleBookMark = new RoutedUICommand();
	}
}
