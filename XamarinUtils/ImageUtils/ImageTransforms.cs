/* Colour lookup tables are based on G'MIC by Pat David. http://gmic.eu/film_emulation/ */

using System;
using System.Linq;

namespace XamarinUtils
{
	public static class ImageTransforms
	{
		static Random random = new Random ();

		public static int[] ApplyCLUT (int[] pixels, int[] clut, int clutWidth)
		{
			int[] output = new int[pixels.Length];

			double cl = Math.Floor (Math.Pow (clutWidth, 1f / 3f) + 0.001);
			double cs = cl * cl;
			double cs1 = cs - 1;

			int a, R, G, B, clutIndex;
			double r, g, b;

			for (int i = 0; i < pixels.Length; i++) {
				a = (pixels [i] >> 24) & 0xff;
				r = ((pixels [i] >> 16) & 0xff) / 255f * cs1;
				g = ((pixels [i] >> 8) & 0xff) / 255f * cs1;
				b = ((pixels [i] >> 0) & 0xff) / 255f * cs1;

				clutIndex = (int)(Dither (b) * cs * cs + Dither (g) * cs + Dither (r));

				R = clut [clutIndex] >> 16 & 0xff;
				G = clut [clutIndex] >> 8 & 0xff;
				B = clut [clutIndex] >> 0 & 0xff;

				output [i] = (a << 24) | (R << 16) | (G << 8) | (B << 0);
			}

			return output;
		}

		static double Dither (double value)
		{
			double floorValue = Math.Floor (value);
			double remainder = value - floorValue;
			return (random.NextDouble () > remainder) ? floorValue : Math.Ceiling (value);
		}

		public static int[] Gamma (int[] src, double red, double green, double blue)
		{
			int a, r, g, b, A, R, G, B;

			int[] gammaR = new int[256];
			int[] gammaG = new int[256];
			int[] gammaB = new int[256];

			int[] output = new int[src.Length];

			for (int i = 0; i < 256; ++i) {
				gammaR [i] = (int)Math.Min (255, ((255 * Math.Pow (i / 255, 1 / red)) + 0.5));
				gammaG [i] = (int)Math.Min (255, ((255 * Math.Pow (i / 255, 1 / green)) + 0.5));
				gammaB [i] = (int)Math.Min (255, ((255 * Math.Pow (i / 255, 1 / blue)) + 0.5));
			}

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = gammaR [r];
				G = gammaG [g];
				B = gammaB [b];

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] ColourFilter (int[] src, double red, double green, double blue)
		{
			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = (int)(r * red);
				G = (int)(g * green);
				B = (int)(b * blue);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] SepiaTone (int[] src, int depth, double red, double green, double blue)
		{
			double GS_RED = 0.3;
			double GS_GREEN = 0.59;
			double GS_BLUE = 0.11;

			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				B = G = R = (int)(GS_RED * r + GS_GREEN * g + GS_BLUE * b);
				R = (int)Math.Min (R + (depth * red), 255);
				G = (int)Math.Min (G + (depth * green), 255);
				B = (int)Math.Min (B + (depth * blue), 255);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] DecreaseColorDepth (int[] src, int bitOffset)
		{
			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = (int)Math.Max (((r + (bitOffset / 2)) - ((r + (bitOffset / 2)) % bitOffset) - 1), 0);
				G = (int)Math.Max (((g + (bitOffset / 2)) - ((g + (bitOffset / 2)) % bitOffset) - 1), 0);
				B = (int)Math.Max (((b + (bitOffset / 2)) - ((b + (bitOffset / 2)) % bitOffset) - 1), 0);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] Contrast (int[] src, double value)
		{
			double contrast = Math.Pow ((100 + value) / 100, 2);

			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = (int)Math.Max (Math.Min ((((((r / 255.0) - 0.5) * contrast) + 0.5) * 255.0), 255), 0);
				G = (int)Math.Max (Math.Min ((((((g / 255.0) - 0.5) * contrast) + 0.5) * 255.0), 255), 0);
				B = (int)Math.Max (Math.Min ((((((b / 255.0) - 0.5) * contrast) + 0.5) * 255.0), 255), 0);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] Brightness (int[] src, int value)
		{
			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = (int)Math.Min (r + value, 255);
				G = (int)Math.Min (g + value, 255);
				B = (int)Math.Min (b + value, 255);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] Boost (int[] src, int type, float percent)
		{
			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = r;
				G = g;
				B = b;

				if (type == 1) {
					R = (int)Math.Min ((r * (1 + percent)), 255);
				} else if (type == 2) {
					G = (int)Math.Min ((g * (1 + percent)), 255);
				} else if (type == 3) {
					R = (int)Math.Min ((b * (1 + percent)), 255);
				}

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] Tint (int[] src, int degree)
		{
			double HALF_CIRCLE_DEGREE = 180d;
			double RANGE = 256d;

			double angle = (Math.PI * (double)degree) / HALF_CIRCLE_DEGREE;

			int S = (int)(RANGE * Math.Sin (angle));
			int C = (int)(RANGE * Math.Cos (angle));

			int a, r, g, b, A, R, G, B, RY, BY, RYY, GYY, BYY, Y;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {
				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				RY = (70 * r - 59 * g - 11 * b) / 100;
				BY = (-30 * r - 59 * g + 89 * b) / 100;
				Y = (30 * r + 59 * g + 11 * b) / 100;
				RYY = (S * BY + C * RY) / 256;
				BYY = (C * BY - S * RY) / 256;
				GYY = (-51 * RYY - 19 * BYY) / 100;

				A = a;
				R = Math.Max (Math.Min (Y + RYY, 255), 0);
				G = Math.Max (Math.Min (Y + GYY, 255), 0);
				B = Math.Max (Math.Min (Y + BYY, 255), 0);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] Shade (int[] src, int shade)
		{
			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {
				output [i] = src [i] & shade;
			}

			return output;
		}
	}
}