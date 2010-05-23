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
		public static string UpgradeLink = "http://phobos.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=366862216";

		public static void Main(string[] args)
		{
			UIApplication.Main(args,null,"AppDelegate");
		}

		/// <summary>
		/// Launches the appstore when an upgrade links is clicked in the lite edition .
		/// </summary>
		public static void LaunchAppstoreProEdition()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(UpgradeLink));
		}
	}
}
