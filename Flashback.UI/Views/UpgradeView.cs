using System;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.UI;

namespace Flashback.UI
{
	public class UpgradeView : UIAlertView
	{
		private UpgradeViewDelegate _delegate;

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
