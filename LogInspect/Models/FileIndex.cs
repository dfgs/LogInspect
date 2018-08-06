﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public struct FileIndex
	{
		public long Position
		{
			get;
			private set;
		}
		public int LineIndex
		{
			get;
			private set;
		}
		public int EventIndex
		{
			get;
			private set;
		}
		public string Severity
		{
			get;
			private set;
		}
		public FileIndex(long Position,int LineIndex,int EventIndex,string Severity)
		{
			this.Position = Position;this.LineIndex = LineIndex;this.EventIndex = EventIndex;this.Severity = Severity;
		}

	}
}
