using System;

using UIKit;
using CoreGraphics;

namespace XamarinUtils.iOS
{
	public static class UIViewUtil
	{
		public static CGRect GetImageFrameInImageView (UIImageView imageView)
		{
			if (imageView.Image == null) {
				return CGRect.Empty;
			}

			nfloat sx = imageView.Frame.Width / imageView.Image.Size.Width;
			nfloat sy = imageView.Frame.Height / imageView.Image.Size.Height;
			nfloat scale;

			switch (imageView.ContentMode) {
			case UIViewContentMode.ScaleAspectFit:
				scale = NMath.Min (sx, sy);

				return new CGRect (
					(imageView.Frame.Width - imageView.Image.Size.Width * scale) / 2,
					(imageView.Frame.Height - imageView.Image.Size.Height * scale) / 2,
					imageView.Image.Size.Width * scale,
					imageView.Image.Size.Height * scale
				);

			case UIViewContentMode.ScaleAspectFill:
				scale = NMath.Max (sx, sy);

				return new CGRect (
					(imageView.Frame.Width - imageView.Image.Size.Width * scale) / 2,
					(imageView.Frame.Height - imageView.Image.Size.Height * scale) / 2,
					imageView.Image.Size.Width * scale,
					imageView.Image.Size.Height * scale
				);

			case UIViewContentMode.ScaleToFill:

				return imageView.Frame;

			default:

				return new CGRect (
					(imageView.Frame.Width - imageView.Image.Size.Width) / 2,
					(imageView.Frame.Height - imageView.Image.Size.Height) / 2,
					imageView.Image.Size.Width,
					imageView.Image.Size.Height
				);
			}
		}
	}
}