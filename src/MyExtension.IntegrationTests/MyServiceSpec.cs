using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Xunit;

[assembly: VsixRunner(TraceLevel = SourceLevels.Verbose)]

namespace MyExtension.IntegrationTests
{
	public class MyServiceSpec
	{
		[VsixFact(VisualStudioVersion.VS2015)]
		public void when_invoking_service_then_succeeds ()
		{
			Trace.WriteLine ("Environment variables in remote Visual Studio process running from: " + Process.GetCurrentProcess ().StartInfo.FileName);
			Trace.WriteLine(string.Join (Environment.NewLine, Environment
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

			var components = GlobalServices.GetService<SComponentModel, IComponentModel>();
			var service = components.GetService<IMyService>();

			service.Run ("foo");
		}
	}
}
