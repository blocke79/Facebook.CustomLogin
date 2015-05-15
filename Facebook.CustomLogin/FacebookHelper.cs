using System;
using Android.Runtime;
using Android.Widget;
using Xamarin.Facebook;
using Object = Java.Lang.Object;

namespace Facebook.CustomLogin
{
	public class FacebookHelper
	{
		public class CustomAcessTokenTracker : AccessTokenTracker
		{
			public delegate void CurrentAccessTokenChangedDelegate (
                AccessToken oldAcessToken, AccessToken currentAcessToken);

			public CurrentAccessTokenChangedDelegate HandleCurrentAccessTokenChanged { get; set; }

			protected override void OnCurrentAccessTokenChanged (AccessToken oldAcessToken, AccessToken currentAcessToken)
			{
				CurrentAccessTokenChangedDelegate p = HandleCurrentAccessTokenChanged;
				if (p != null)
					p (oldAcessToken, currentAcessToken);

				if (currentAcessToken != null) {
					MyApplication.FacebookAccessToken = currentAcessToken.Token;
				} else {
					MyApplication.FacebookAccessToken = null;
				}
//				Toast.MakeText (Android.App.Application.Context, "Facebook AccessTokenUpdated", ToastLength.Short)
//                    .Show ();
			}
		}

		public class CustomProfileTracker : ProfileTracker
		{
			public delegate void CurrentProfileChangedDelegate (Profile oldProfile, Profile currentProfile);

			public CurrentProfileChangedDelegate HandleCurrentProfileChanged { get; set; }

			protected override void OnCurrentProfileChanged (Profile oldProfile, Profile currentProfile)
			{
				CurrentProfileChangedDelegate p = HandleCurrentProfileChanged;
				if (p != null)
					p (oldProfile, currentProfile);

				if (currentProfile != null) {
					MyApplication.FacebookId = currentProfile.Id;
				} else {
					MyApplication.FacebookId = null;
				}
			}
		}

		public class FacebookCallback<TResult> : Object, IFacebookCallback where TResult : Object
		{
			public Action HandleCancel { get; set; }

			public Action<FacebookException> HandleError { get; set; }

			public Action<TResult> HandleSuccess { get; set; }

			public void OnCancel ()
			{
				Action c = HandleCancel;
				if (c != null)
					c ();
			}

			public void OnError (FacebookException error)
			{
				Action<FacebookException> c = HandleError;
				if (c != null)
					c (error);
			}

			public void OnSuccess (Object result)
			{
				Action<TResult> c = HandleSuccess;
				if (c != null)
					c (result.JavaCast<TResult> ());
			}
		}
	}
}