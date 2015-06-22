using System;
using UIKit;
using CoreGraphics;

namespace XamarinUtils.iOS
{
	public class TestController : UIViewController
	{
		UIImage image = UIImage.FromFile ("Manual-mode-20MP.jpg");

		UIButton button;
		UIImageView imageView1, imageView2;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			imageView1 = new UIImageView ();
			imageView1.Image = image;
			imageView1.Frame = new CGRect (0, 0, View.Frame.Width, image.Size.Height * (View.Frame.Width / image.Size.Width));

			imageView2 = new UIImageView ();
			imageView2.Frame = new CGRect (0, View.Frame.Height - image.Size.Height * (View.Frame.Width / image.Size.Width), View.Frame.Width, image.Size.Height * (View.Frame.Width / image.Size.Width));

			button = UIButton.FromType (UIButtonType.System);
			button.BackgroundColor = UIColor.Blue;
			button.Frame = new CGRect (0, imageView1.Frame.Height, View.Frame.Width, View.Frame.Height - imageView1.Frame.Height - imageView2.Frame.Height);
			button.TouchUpInside += Transform;

			View.AddSubviews (imageView1, imageView2, button);
		}

		void Transform (object sender, EventArgs e)
		{
			InvokeInBackground (delegate {
				int[] pixels = ImageUtils.GetPixelData (image);

				InvokeOnMainThread (delegate {
					imageView2.Image = ImageUtils.GetImage (pixels);
				});
			});
		}
	}
}