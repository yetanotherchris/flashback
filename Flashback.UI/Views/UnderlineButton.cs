using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace Flashback.UI
{
	/// <summary>
	/// A hyperlink style button, with works best with no border.
	/// </summary>
	public class UnderlineButton : UIButton
	{		
		public override void Draw(RectangleF rect)
		{
			base.Draw (rect);
			
			// Red underline
			CGContext context = UIGraphics.GetCurrentContext();
			context.SetRGBFillColor(0xFF,0xFF,0,1);
			context.SetLineWidth(1);
			context.MoveTo(0,rect.Height -5);
			context.AddLineToPoint(rect.Width,rect.Height -5);
			context.StrokePath();	
		}
	}
}
