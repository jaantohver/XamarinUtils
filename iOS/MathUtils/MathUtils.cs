using System;

namespace XamarinUtils.iOS
{
	public static class MathUtils
	{
		public static nfloat AffineTransform (nfloat a, nfloat b, nfloat c, nfloat d, nfloat x)
		{
			return (x - a) * ((d - c) / (b - a)) + c;
		}

		public static int AffineTransformRounded (nfloat a, nfloat b, nfloat c, nfloat d, nfloat x)
		{
			return (int)NMath.Round (AffineTransform (a, b, c, d, x));
		}
	}
}