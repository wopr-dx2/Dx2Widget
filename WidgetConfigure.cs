using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dx2Widget
{
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_CONFIGURE" })]
    class WidgetConfigure : PreferenceActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
            AddPreferencesFromResource(Resource.Xml.preferences);
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
        }
    }
}