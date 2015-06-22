namespace XamarinUtils
{
	public static class ImageFilters
	{
		public static int[] Invert (int[] src)
		{
			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = 255 - r;
				G = 255 - g;
				B = 255 - b;

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}

		public static int[] Grayscale (int[] src)
		{
			double GS_RED = 0.299;
			double GS_GREEN = 0.587;
			double GS_BLUE = 0.114;

			int a, r, g, b, A, R, G, B;

			int[] output = new int[src.Length];

			for (int i = 0; i < src.Length; i++) {

				a = (src [i] >> 24) & 0xff;
				r = (src [i] >> 16) & 0xff;
				g = (src [i] >> 8) & 0xff;
				b = (src [i] >> 0) & 0xff;

				A = a;
				R = G = B = (int)(GS_RED * r + GS_GREEN * g + GS_BLUE * b);

				output [i] = (A << 24) | (R << 16) | (G << 8) | B;
			}

			return output;
		}
	}
}