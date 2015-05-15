using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace Facebook.CustomLogin
{
    [Activity(Label = "Authenticated")]
    public class AuthenticatedActivity : BaseActivity
    {
        private TextView _accesstoken;
        private Button _logoutBtn;
        private ProfilePictureView _profilephoto;
        private TextView _userId;

        protected override int LayoutResource
        {
            get { return Resource.Layout.activity_authenticated; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _userId = FindViewById<TextView>(Resource.Id.userId);
            _accesstoken = FindViewById<TextView>(Resource.Id.accesstoken);
            _profilephoto = FindViewById<ProfilePictureView>(Resource.Id.profilePicture);
            _logoutBtn = FindViewById<Button>(Resource.Id.logoutBtn);

            _logoutBtn.Click += delegate
            {
                LoginManager.Instance.LogOut();
                Toast.MakeText(this, "Logout Success", ToastLength.Short).Show();

                var intent = new Intent(this, typeof (MainActivity));
                StartActivity(intent);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            UpdateUi();
        }


        private void UpdateUi()
        {
            if (MyApplication.FacebookAccessToken != null)
            {
                _userId.Text = MyApplication.FacebookId;
                _accesstoken.Text = MyApplication.FacebookAccessToken;
                _logoutBtn.Visibility = ViewStates.Visible;

                if (MyApplication.FacebookId != null)
                {
                    _profilephoto.ProfileId = MyApplication.FacebookId;
                }
            }
        }
    }
}