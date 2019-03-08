using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class ColumnFormatViewModel : ViewModel
	{
		private ColumnViewModel column;

		public string Name
		{
			get;
			set;
		}

		private string format;
		public string Format
		{
			get { return format; }
			set {format = value; }
		}

		public ColumnFormatViewModel(ILogger Logger, ColumnViewModel Column) : base(Logger)
		{
			AssertParameterNotNull("Column", Column);
			this.column = Column;
		}

		public void ApplyNewFormat()
		{
			this.column.Format = Format;
		}


	}
}
