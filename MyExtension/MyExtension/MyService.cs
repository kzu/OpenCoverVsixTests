using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtension
{
	[Export(typeof(IMyService))]
	internal class MyService : IMyService
	{
		public void Run (string message)
		{
			if (string.IsNullOrEmpty (message))
				throw new ArgumentException ("message");

			Debug.WriteLine (message);
		}
	}
}
