﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogInspectLib.Readers
{
    public class EventReader :Reader<Event>
    {
		public override bool EndOfStream
		{
			get { return logReader.EndOfStream; }
		}

		public override long Position
		{
			get { return logReader.Position; }
		}

		private LogReader logReader;

		private List<LogParser> logParsers;
		public FormatHandler FormatHandler { get; }

		

		
		public EventReader(Stream Stream, Encoding Encoding,int BufferSize, FormatHandler FormatHandler):base()
        {
			if (Stream == null) throw new ArgumentNullException("Stream");
			if (Encoding == null) throw new ArgumentNullException("Encoding");
			if (BufferSize <= 0) throw new ArgumentException("BufferSize");
			if (FormatHandler == null) throw new ArgumentNullException("FormatHandler");

			this.logReader = new LogReader(Stream, Encoding, BufferSize,FormatHandler);

			this.FormatHandler = FormatHandler;

			logParsers = new List<LogParser>();			
			foreach (Rule rule in FormatHandler.Rules)
			{
				this.logParsers.Add(new LogParser(rule));
			}
			
			
		}

		public override void Seek(long Position)
		{
			logReader.Seek(Position);
		}
		

		public override Event Read()
		{
			Log log;
			Event ev;

			log = logReader.Read();
			foreach(LogParser parser in logParsers)
			{
				ev = parser.Parse(log);
				if (ev!=null) return ev;
			}

			ev = new Event();
			ev.Log = log;
			return ev;
		}



    }
}