using System;

namespace XamarinUtils
{
	public static class MathUtils
	{
		public static int AffineTransform (int a, int b, int c, int d, int x)
		{
			return (x - a) * ((d - c) / (b - a)) + c;
		}

		public static float AffineTransform (float a, float b, float c, float d, float x)
		{
			return (x - a) * ((d - c) / (b - a)) + c;
		}

		public static double AffineTransform (double a, double b, double c, double d, double x)
		{
			return (x - a) * ((d - c) / (b - a)) + c;
		}

		public static int AffineTransformRounded (float a, float b, float c, float d, float x)
		{
			return (int)Math.Round (AffineTransform (a, b, c, d, x));
		}

		public static int AffineTransformRounded (double a, double b, double c, double d, double x)
		{
			return (int)Math.Round (AffineTransform (a, b, c, d, x));
		}
	}
}