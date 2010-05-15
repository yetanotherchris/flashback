using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;

namespace Flashback.UI
{
	public class Application
	{
		public static string ProEditionLink = "";

		public static void Main(string[] args)
		{
			UIApplication.Main(args,null,"AppDelegate");
		}

		public static void LaunchAppstoreProEdition()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl("http://phobos.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=301349397"));
		}
	}
}
