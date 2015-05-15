using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Login;

[assembly: Permission(Name = Manifest.Permission.Internet)]
[assembly: Permission(Name = Manifest.Permission.WriteExternalStorage)]
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]

namespace Facebook.CustomLogin
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        private AccessTokenTracker _accesstokenTracker;
        private ICallbackManager _callbackManager;
        private Button _loginBtn;
        private ProgressDialog _loginprogress;
        private Button _logoutBtn;
        private ProfileTracker _profileTracker;


        protected override int LayoutResource
        {
            get { return Resource.Layout.activity_main; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Resources.GetString(Resource.String.app_id) == "your-app-id")
            {
                Toast.MakeText(this, "You need to add you Facebook App id to the strings.xml file", ToastLength.Long).Show();
            }

            _loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
            _logoutBtn = FindViewById<Button>(Resource.Id.logoutBtn);

            FacebookSdk.SdkInitialize(ApplicationContext);

            _callbackManager = CallbackManagerFactory.Create();

            var loginCallback = new FacebookHelper.FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
                    UpdateVars();

                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();

                    var intent = new Intent(this, typeof (AuthenticatedActivity));
                    StartActivity(intent);
                },
                HandleCancel = () =>
                {
                    Toast.MakeText(this, "Login Cancel", ToastLength.Short).Show();
                    _loginprogress.Dispose();
                },
                HandleError = loginError =>
                {
                    if (loginError is FacebookAuthorizationException)
                    {
                        Toast.MakeText(this, loginError.Message, ToastLength.Long).Show();
                    }

                    UpdateVars();
                    _loginprogress.Dispose();
                }
            };

            LoginManager.Instance.RegisterCallback(_callbackManager, loginCallback);

            _profileTracker = new FacebookHelper.CustomProfileTracker
            {
                HandleCurrentProfileChanged = (oldProfile, currentProfile) => UpdateVars()
            };
            _profileTracker.StartTracking();

            _accesstokenTracker = new FacebookHelper.CustomAcessTokenTracker
            {
                HandleCurrentAccessTokenChanged = (oldAccessToken, currentAccessToken) => UpdateVars()
            };
            _accesstokenTracker.StartTracking();

            var permissionList = new List<string>
            {
                "public_profile",
                "user_friends"
            };

            _loginBtn.Click += delegate
            {
                _loginprogress = ProgressDialog.Show(this, "", "Loading...", true);
                _loginprogress.SetProgressStyle(ProgressDialogStyle.Horizontal);
                LoginManager.Instance.LogInWithReadPermissions(this, permissionList);
                UpdateUi();
            };

            _logoutBtn.Click += delegate
            {
                LoginManager.Instance.LogOut();
                UpdateUi();
            };

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
        }

        protected override void OnResume()
        {
            base.OnResume();

            AppEventsLogger.ActivateApp(this);

            UpdateUi();
            UpdateVars();
        }


        protected override void OnPause()
        {
            base.OnPause();

            AppEventsLogger.DeactivateApp(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _accesstokenTracker.StopTracking();
            _profileTracker.StopTracking();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _callbackManager.OnActivityResult(requestCode, (int) resultCode, data);
        }

        private void UpdateUi()
        {
            if (MyApplication.FacebookAccessToken != null)
            {
                _loginBtn.Visibility = ViewStates.Gone;
                _logoutBtn.Visibility = ViewStates.Visible;
            }
            else
            {
                _loginBtn.Visibility = ViewStates.Visible;
                _logoutBtn.Visibility = ViewStates.Gone;
            }
        }

        private void UpdateVars()
        {
            if (AccessToken.CurrentAccessToken != null)
            {
                MyApplication.FacebookAccessToken = AccessToken.CurrentAccessToken.Token;
                MyApplication.FacebookId = AccessToken.CurrentAccessToken.UserId;
            }
            else
            {
                MyApplication.FacebookAccessToken = null;
            }
        }
    }
}