using LogInspectLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public interface IVirtualCollection<T>: INotifyPropertyChanged,  IList<T>,IList,INotifyCollectionChanged
	{
		event EventHandler CountChanged;

		void SetCount(int Value);



	}
}
