using System;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;

namespace XamarinUtils.Droid
{
	[Activity (Label = "XamarinUtils.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class ImageUtilsTest : Activity
	{
		static Bitmap originalImage;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			originalImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.big, new BitmapFactory.Options {
				InSampleSize = 8
			});

			SetContentView (new ContentView (this));
		}

		private class ContentView : RelativeLayout
		{
			ListView list;
			ImageView image;

			public ContentView (Activity context) : base (context)
			{
				image = new ImageView (context);
				image.SetImageBitmap (originalImage);

				list = new ListView (context);
				list.Adapter = new Adapter (context);
				list.ItemClick += OnItemClick;

				AddView (image);
				AddView (list);
			}

			protected override void OnLayout (bool changed, int left, int top, int right, int bottom)
			{
				base.OnLayout (changed, left, top, right, bottom);

				if (changed) {
					int w = right - left;
					int h = bottom - top;

					image.Layout (0, 0, w, h / 2);

					list.Layout (0, h / 2, w, h);
				}
			}

			void OnItemClick (object sender, AdapterView.ItemClickEventArgs e)
			{
				Bitmap clutImage = null;

				switch (e.Position) {
				case 0:
					image.SetImageBitmap (originalImage);
					return;
				case 1:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.black_and_white);
					break;
				case 2:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.color_rich);
					break;
				case 3:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.faded);
					break;
				case 4:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.faded_alt);
					break;
				case 5:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.faded_analog);
					break;
				case 6:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.faded_extreme);
					break;
				case 7:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.faded_vivid);
					break;
				case 8:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.hong_kong);
					break;
				case 9:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.light_brown);
					break;
				case 10:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.lomo);
					break;
				case 11:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.natural_vivid);
					break;
				case 12:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.nostalgic);
					break;
				case 13:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.purple);
					break;
				case 14:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.retro);
					break;
				case 15:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sixties);
					break;
				case 16:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sixties_faded);
					break;
				case 17:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sixties_faded_alt);
					break;
				case 18:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.summer);
					break;
				case 19:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.summer_alt);
					break;
				case 20:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sunny);
					break;
				case 21:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sunny_alt);
					break;
				case 22:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sunny_rich);
					break;
				case 23:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sunny_warm);
					break;
				case 24:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.super_warm);
					break;
				case 25:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.super_warm_rich);
					break;
				case 26:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.sutro_fx);
					break;
				case 27:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.warm);
					break;
				case 28:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.warm_yellow);
					break;
				case 29:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.vibrant);
					break;
				case 30:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.vibrant_alien);
					break;
				case 31:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.vintage);
					break;
				case 32:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.vintage_alt);
					break;
				case 33:
					clutImage = BitmapFactory.DecodeResource (Resources, Resource.Drawable.vintage_brighter);
					break;
				}

				int[] pixels = ImageUtils.GetPixelData (originalImage);

				int[] clutPixels = ImageUtils.GetPixelData (clutImage);

				int[] transformedPixels = ImageTransforms.ApplyCLUT (pixels, clutPixels, clutImage.Width);

				Bitmap transformedImage = ImageUtils.GetImage (transformedPixels, originalImage.Width, originalImage.Height, originalImage.GetConfig ());

				image.SetImageBitmap (transformedImage);
			}
		}

		private class Adapter : BaseAdapter
		{
			Activity context;

			public Adapter (Activity context)
			{
				this.context = context;
			}

			public override Java.Lang.Object GetItem (int position)
			{
				return null;
			}

			public override long GetItemId (int position)
			{
				return 0;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				TextView view = convertView as TextView ?? new TextView (context);

				view.Text = GetText (position);
				view.LayoutParameters = new ViewGroup.LayoutParams (parent.Width, 100);

				return view;
			}

			public override int Count {
				get {
					return 34;
				}
			}

			string GetText (int position)
			{
				switch (position) {
				case 0:
					return "none";
				case 1:
					return "black_and_white";
				case 2:
					return "color_rich";
				case 3:
					return "faded";
				case 4:
					return "faded_alt";
				case 5:
					return "faded_analog";
				case 6:
					return "faded_extreme";
				case 7:
					return "faded_vivid";
				case 8:
					return "hong_kong";
				case 9:
					return "light_brown";
				case 10:
					return "lomo";
				case 11:
					return "natural_vivid";
				case 12:
					return "nostalgic";
				case 13:
					return "purple";
				case 14:
					return "retro";
				case 15:
					return "sixties";
				case 16:
					return "sixties_faded";
				case 17:
					return "sixties_faded_alt";
				case 18:
					return "summer";
				case 19:
					return "summer_alt";
				case 20:
					return "sunny";
				case 21:
					return "sunny_alt";
				case 22:
					return "sunny_rich";
				case 23:
					return "sunny_warm";
				case 24:
					return "super_warm";
				case 25:
					return "super_warm_rich";
				case 26:
					return "sutro_fx";
				case 27:
					return "warm";
				case 28:
					return "warm_yellow";
				case 29:
					return "vibrant";
				case 30:
					return "vibrant_alien";
				case 31:
					return "vintage";
				case 32:
					return "vintage_alt";
				case 33:
					return "vintage_brighter";
				default:
					return "N/A";
				}
			}
		}
	}
}