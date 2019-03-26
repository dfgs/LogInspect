using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Test
{
	[TestClass]
	public class StreamProgressReporterUnitTest
    {
		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(()=>new StreamProgressReporter(null));
		}

		[TestMethod]
		public void ShouldReportCorrectProgress()
		{
			StreamProgressReporter streamProgressReporter;
			MemoryStream stream;
			byte[] buffer;


			stream = new MemoryStream(new byte[10]);
			streamProgressReporter = new StreamProgressReporter(stream);

			buffer = new byte[1];

			for (int t = 0; t < 10; t++)
			{
				Assert.AreEqual(t, streamProgressReporter.Position);
				Assert.AreEqual(10, streamProgressReporter.Length);
				stream.Read(buffer, 0, 1);
			}

		}



	}
}
