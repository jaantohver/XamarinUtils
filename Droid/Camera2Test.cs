using System;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Hardware.Camera2;

namespace XamarinUtils.Droid
{
	[Activity]
	public class Camera2TestActivity : Activity
	{
		Camera2TestView contentView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RequestWindowFeature (WindowFeatures.NoTitle);

			Window.SetFlags
			(
				WindowManagerFlags.KeepScreenOn | WindowManagerFlags.HardwareAccelerated,
				WindowManagerFlags.KeepScreenOn | WindowManagerFlags.HardwareAccelerated
			);

			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
			    SystemUiFlags.Fullscreen |
			    SystemUiFlags.HideNavigation |
			    SystemUiFlags.ImmersiveSticky
			);

			contentView = new Camera2TestView (this);

			SetContentView (contentView);
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			contentView.CameraView.OpenBackCamera ();

			contentView.Flash.Click += ChangeFlash;
			contentView.SwitchCamera.Click += SwitchCamera;
			contentView.TakePicture.Click += TakePicture;
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			contentView.CameraView.CloseCamera ();

			contentView.Flash.Click -= ChangeFlash;
			contentView.SwitchCamera.Click -= SwitchCamera;
			contentView.TakePicture.Click -= TakePicture;
		}

		void ChangeFlash (object sender, EventArgs e)
		{
			contentView.CameraView.ChangeFlash (FlashMode.Torch);
		}

		void SwitchCamera (object sender, EventArgs e)
		{
			contentView.CameraView.OpenFrontCamera ();
		}

		void TakePicture (object sender, EventArgs e)
		{
			contentView.CameraView.TakePicture ();
		}
	}

	class Camera2TestView : RelativeLayout
	{
		public readonly Button Flash, SwitchCamera, TakePicture;
		public readonly Camera2View CameraView;

		public Camera2TestView (Camera2TestActivity context) : base (context)
		{
			CameraView = new Camera2View (context);

			Flash = new Button (context);
			Flash.Text = "Flash";

			SwitchCamera = new Button (context);
			SwitchCamera.Text = "Switch Camera";

			TakePicture = new Button (context);
			TakePicture.Text = "Take Picture";

			AddView (CameraView);
			AddView (Flash);
			AddView (SwitchCamera);
			AddView (TakePicture);
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);

			CameraView.Layout (l, t, r, b);

			Flash.Layout (0, 0, 400, 100);

			SwitchCamera.Layout (0, Flash.Bottom, 400, Flash.Bottom + 100);

			TakePicture.Layout (0, SwitchCamera.Bottom, 400, SwitchCamera.Bottom + 100);
		}
	}
}