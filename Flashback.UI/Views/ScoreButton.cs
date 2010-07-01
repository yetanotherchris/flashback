using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace Flashback.UI
{
	/// <summary>
	/// A button for scoring a question with.
	/// </summary>
	public class ScoreButton : UIButton
	{		
		public int Score { get; set; }
	}
}
