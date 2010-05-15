using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Flashback.UI
{
	public class BusyView : UIAlertView
	{
		private UIActivityIndicatorView _activityView;

		public void Show(string title)
		{
			Title = title;
			Show();

			// Spinner - add after Show() or we have no Bounds.
			_activityView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			_activityView.Frame = new RectangleF((Bounds.Width / 2) - 15, Bounds.Height - 50, 30, 30);
			_activityView.StartAnimating();
			AddSubview(_activityView);
		}

		public void Hide()
		{
			DismissWithClickedButtonIndex(0, true);
		}
	}
}
