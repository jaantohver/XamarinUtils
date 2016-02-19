using Android.App;

namespace XamarinUtils.Droid
{
	[Activity (MainLauncher = true, NoHistory = true)]
	public class GenericSplashScreen : Activity
	{
		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			StartActivity (typeof(Camera2TestActivity));
		}
	}
}