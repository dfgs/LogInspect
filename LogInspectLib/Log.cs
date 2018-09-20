﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LogInspectLib
{
    public class Log
    {
        public List<Line> Lines
        {
            get;
            private set;
        }
		
		public long Position
		{
			get { return Lines.FirstOrDefault()?.Position??-1; }
		}

		public int LineIndex
		{
			get { return Lines.FirstOrDefault()?.Index ?? -1; }
		}

		public Log()
		{
			Lines = new List<Line>();
		}
		

		public string ToSingleLine()
		{
			return String.Join("", Lines.Select(item=>item.Value));
		}

		public string ToMultipleLines()
		{
			return String.Join("\r\n", Lines.Select(item => item.Value));
		}

		public override string ToString()
		{
			return ToSingleLine();
		}
	}
}
