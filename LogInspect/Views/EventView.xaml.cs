﻿using LogInspect.ViewModels;
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

namespace LogInspect.Views
{
	/// <summary>
	/// Logique d'interaction pour EventView.xaml
	/// </summary>
	public partial class EventView : UserControl
	{

		public EventView()
		{
			InitializeComponent();
		}

		private void ListView_Selected(object sender, SelectionChangedEventArgs e)
		{
			ListView listView;
			listView = sender as ListView;
			if (listView == null) return;
			listView.ScrollIntoView(listView.SelectedItem);
		}

		
	}
}
