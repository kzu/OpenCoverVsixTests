using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace MyExtension.MsTests
{
	[TestClass]
	public class MyServiceSpec
	{
		[TestProperty ("VsHiveName", "14.0Exp")]
		[HostType ("VS IDE")]
		[TestMethod]
		public void when_invoking_service_then_succeeds ()
		{
			Trace.WriteLine ("Environment variables in remote Visual Studio process running the integration test: ");
			Trace.WriteLine (string.Join (Environment.NewLine, Environment
				.GetEnvironmentVariables ()
				.OfType<DictionaryEntry> ()
				.OrderBy (x => (string)x.Key)
				.Where (x =>
					!((string)x.Key).Equals ("path", StringComparison.OrdinalIgnoreCase) &&
					!((string)x.Key).Equals ("include", StringComparison.OrdinalIgnoreCase) &&
					!((string)x.Key).Equals ("pathbackup", StringComparison.OrdinalIgnoreCase))
				.Select (x => "    " + x.Key + "=" + x.Value)));

			//Trace.WriteLine ("COMPLUS_ProfAPI_ProfilerCompatibilitySetting = " + Environment.GetEnvironmentVariable ("COMPLUS_ProfAPI_ProfilerCompatibilitySetting"));
			//Trace.WriteLine ("CLRMONITOR_EXTERNAL_PROFILERS = "+ Environment.GetEnvironmentVariable ("CLRMONITOR_EXTERNAL_PROFILERS"));

			var components = (IComponentModel)VsIdeTestHostContext.ServiceProvider.GetService(typeof(SComponentModel));
			var service = components.GetService<IMyService>();

			service.Run ("foo");
		}
	}
}
