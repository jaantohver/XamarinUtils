using System;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Runtime.InteropServices;

namespace XamarinUtils.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			UIWindow window = new UIWindow (UIScreen.MainScreen.Bounds);

			UINavigationController navController = new UINavigationController ();
			navController.NavigationBarHidden = true;
			navController.NavigationBar.Translucent = false;

			window.RootViewController = navController;
			window.MakeKeyAndVisible ();

			navController.PresentViewController (new TestController (), false, delegate {
			});

			return true;
		}
	}
}