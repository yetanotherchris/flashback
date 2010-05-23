using System;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.UI;

namespace Flashback.UI
{
	/// <summary>
	/// Modal dialog asking if the user wants to upgrade.
	/// </summary>
	public class UpgradeView : UIAlertView
	{
		private UpgradeViewDelegate _delegate;

		/// <summary>
		/// Shows the dialog view with the message plus "Do you want to upgrade now?"
		/// </summary>
		/// <param name="message"></param>
		public void Show(string message)
		{
			Title = "You need to upgrade for this feature";
			Message = message +"\nDo you want to upgrade now?";

			_delegate = new UpgradeViewDelegate();
			Delegate = _delegate;
			AddButton("Yes");
			AddButton("No Thanks");
			Show();
		}

		private class UpgradeViewDelegate : UIAlertViewDelegate
		{
			public override void Clicked(UIAlertView alertview, int buttonIndex)
			{
				if (buttonIndex == 0)
				{
					Application.LaunchAppstoreProEdition();
				}
			}
		}
	}
}
