using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace Facebook.CustomLogin
{
	public abstract class BaseActivity : ActionBarActivity
	{
		public Toolbar Toolbar {
			get;
			set;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (LayoutResource);
			Toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			if (Toolbar != null) {
				SetSupportActionBar (Toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled (false);
                SupportActionBar.SetHomeButtonEnabled(false);

			}
		}

		protected abstract int LayoutResource {
			get;
		}

		protected int ActionBarIcon {
			set { Toolbar.SetNavigationIcon (value); }
		}
	}

}