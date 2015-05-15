using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Runtime;
using Object = Java.Lang.Object;

namespace Facebook.CustomLogin
{
	public class MyApplication : Application
	{
		#region vars

		public static string FacebookId;
		public static string FacebookAccessToken;

		#endregion

		protected readonly string LogTag = "App";

		public MyApplication (IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer)
		{
		}

		public static Context ProvidedContext { get; private set; }


		public static ISharedPreferences Preference { get; private set; }

		public override void OnCreate ()
		{
			base.OnCreate ();

			ProvidedContext = ApplicationContext;

			Preference = PreferenceManager.GetDefaultSharedPreferences (ProvidedContext);
		}

		public static T GetManager<T> (string name) where T : Object
		{
			return (ProvidedContext.GetSystemService (name) as T);
		}


		public static bool IsAuthenticated ()
		{
			return !string.IsNullOrEmpty (FacebookAccessToken);
		}
	}
}