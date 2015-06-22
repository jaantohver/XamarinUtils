using System;

namespace XamarinUtils
{
	#if __ANDROID__

	using Android.Graphics;

	public static class ImageUtils
	{
		public static int [] GetPixelData (Bitmap image)
		{
			int w = image.Width;
			int h = image.Height;

			int[] pixels = new int[w * h];

			image.GetPixels (pixels, 0, w, 0, 0, w, h);

			return pixels;
		}

		public static Bitmap GetImage (int[] pixels, int w, int h, Bitmap.Config config)
		{
			Bitmap bmp = Bitmap.CreateBitmap (w, h, config);

			bmp.SetPixels (pixels, 0, w, 0, 0, w, h);

			return bmp;
		}

		public static Bitmap Resize (Bitmap bmp, int w, int h)
		{
			return Bitmap.CreateScaledBitmap (bmp, w, h, false);
		}

		public static Bitmap DecodeAndResizeImage (byte[] bytes, int width, int height)
		{
			BitmapFactory.Options opts = new BitmapFactory.Options ();
			opts.InJustDecodeBounds = true;

			BitmapFactory.DecodeByteArray (bytes, 0, bytes.Length, opts);

			opts.InSampleSize = CalculateInSampleSize (opts, width, height);
			opts.InJustDecodeBounds = false;

			return BitmapFactory.DecodeByteArray (bytes, 0, bytes.Length, opts);
		}

		static int CalculateInSampleSize (BitmapFactory.Options opts, int reqWidth, int reqHeight)
		{
			int height = opts.OutHeight;
			int width = opts.OutWidth;
			int inSampleSize = 1;

			if (height > reqHeight || width > reqWidth) {
				int halfHeight = height / 2;
				int halfWidth = width / 2;

				while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth) {
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}
	}

	#elif __IOS__

	using UIKit;
	using CoreGraphics;

	public static class ImageUtils
	{
		public static int [] GetPixelData (UIImage image)
		{
			int bitsInByte = 8;
			int bytesInPixel = 4;

			int w = (int)image.Size.Width;
			int h = (int)image.Size.Height;

			int[] pixels = new int[w * h];

			byte[] rawData = new byte[w * h * bytesInPixel];

			using (CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB ()) {

				CGBitmapContext context = new CGBitmapContext (
					                        rawData,
					                        w,
					                        h,
					                        bitsInByte,
					                        w * bytesInPixel,
					                        colorSpace,
					                        CGBitmapFlags.PremultipliedFirst | CGBitmapFlags.ByteOrder32Big
				                        );

				context.DrawImage (new CGRect (0, 0, w, h), image.CGImage);

				for (int i = 0, j = 0; i < w * h; i++, j += bytesInPixel) {

					int red = (rawData [j]);
					int green = (rawData [j + 1]);
					int blue = (rawData [j + 2]);
					int alpha = (rawData [j + 3]);

					pixels [i] = (alpha << 24) | (red << 16) | (green << 8) | blue;
				}

				return pixels;
			}
		}

		public static UIImage GetImage (int[] pixels)
		{
			int bitsInByte = 8;
			int bytesInPixel = 4;

			int w = (int)Math.Sqrt ((pixels.Length * 4) / 3);
			int h = (int)(w * (3f / 4f));

			byte[] rawData = new byte[w * h * bytesInPixel];

			for (int i = 0, j = 0; i < w * h; i++, j += bytesInPixel) {

				rawData [j] = (byte)((pixels [i] >> 16) & 0xff);
				rawData [j + 1] = (byte)((pixels [i] >> 8) & 0xff);
				rawData [j + 2] = (byte)((pixels [i] >> 0) & 0xff);
				rawData [j + 3] = (byte)((pixels [i] >> 24) & 0xff);
			}

			CGDataProvider provider = new CGDataProvider (rawData, 0, pixels.Length * bytesInPixel);

			using (CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB ()) {

				CGImage image = new CGImage (
					              w,
					              h,
					              bitsInByte,
					              bitsInByte * bytesInPixel,
					              bytesInPixel * w,
					              colorSpace,
					              CGBitmapFlags.PremultipliedFirst | CGBitmapFlags.ByteOrder32Big,
					              provider,
					              null,
					              false,
					              CGColorRenderingIntent.Default
				              );

				UIImage output = UIImage.FromImage (image);

				return output;
			}
		}
	}

	#endif
}