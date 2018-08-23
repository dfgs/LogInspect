﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
		public override long Length
		{
			get { return logReader.Length; }
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
				this.logParsers.Add(new LogParser(rule,FormatHandler.Columns));
			}
			
			
		}

		protected override void OnSeek(long Position)
		{
			logReader.Seek(Position);
		}

		public int GetReadLines()
		{
			return logReader.GetReadLines();
		}
		protected override Event OnRead()
		{
			Log log;
			Event? ev;

			log = logReader.Read();
			foreach(LogParser parser in logParsers)
			{
				ev = parser.Parse(log);
				if (ev.HasValue) return ev.Value;
			}

			return new Event(log,null, Property.EmptyProperties);
			
		}
		protected override async Task<Event> OnReadAsync()
		{
			Log log;
			Event? ev;

			log = await logReader.ReadAsync();
			foreach (LogParser parser in logParsers)
			{
				ev = parser.Parse(log);
				if (ev.HasValue) return ev.Value;
			}

			return new Event(log, null, Property.EmptyProperties);
		}


	}
}
