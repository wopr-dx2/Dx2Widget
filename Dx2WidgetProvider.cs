using System;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Widget;

// reference

// Xamarin / Simple Widget
// https://developer.xamarin.com/samples/monodroid/SimpleWidget/

// とっても暇でしの部屋
// http://tottemohimadesi.esy.es/android/android-widget-日時＆電池容量/

// Microsoft / Xamarin.Android でブロードキャスト レシーバー
// https://docs.microsoft.com/ja-jp/xamarin/android/app-fundamentals/broadcast-receivers

namespace Dx2Widget
{
	[BroadcastReceiver (Label = "@string/widget_name")]
	[IntentFilter (new string [] { "android.appwidget.action.APPWIDGET_UPDATE" })]
	[MetaData ("android.appwidget.provider", Resource = "@xml/dx2_widget")]
	public class Dx2WidgetProvider : AppWidgetProvider
	{
        private static readonly string ACTION_INTERVAL = "jp.wopr.dx2.widget.INTERVAL";
        private static readonly long INTERVAL = 60 * 1000;

        #region private methods

        PendingIntent getAlarmPendingIntent(Context context)
        {
            Intent intent = new Intent(context, Java.Lang.Class.FromType(typeof(Dx2WidgetProvider)));
            intent.SetAction(ACTION_INTERVAL);
            return PendingIntent.GetBroadcast(context, 0, intent, 0);
        }

        void setInterval(Context context, long interval)
        {
            PendingIntent pendingIntent = getAlarmPendingIntent(context);
            AlarmManager alarmManager =
                (AlarmManager)context.GetSystemService(Context.AlarmService);
            long now = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            long oneHourAfter = ((long)(now / interval)) * interval + interval;

            alarmManager.Set(AlarmType.Rtc, oneHourAfter, pendingIntent);
        }

        void updateTime(Context context)
        {
            setInterval(context, INTERVAL);
        }

        #endregion

        #region overrides

        public override void OnEnabled(Context context)
        {
            base.OnEnabled(context);
        }

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            Intent intent =
                new Intent(context, Java.Lang.Class.FromType(typeof(Dx2Service)));
            context.StartService(intent);

            setInterval(context, INTERVAL);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (ACTION_INTERVAL.Equals(intent.Action))
            {
                updateTime(context);
            }
            base.OnReceive(context, intent);
        }

        public override void OnDeleted(Context context, int[] appWidgetIds)
        {
            base.OnDeleted(context, appWidgetIds);
        }

        public override void OnDisabled(Context context)
        {
            Intent intent = new Intent(context, Java.Lang.Class.FromType(typeof(Dx2Service)));
            context.StopService(intent);

            PendingIntent pendingIntent = getAlarmPendingIntent(context);
            AlarmManager alarmManager =
                (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarmManager.Cancel(pendingIntent);

            base.OnDisabled(context);
        }

        public override void OnAppWidgetOptionsChanged(Context context, AppWidgetManager appWidgetManager, int appWidgetId, Bundle newOptions)
        {
            base.OnAppWidgetOptionsChanged(context, appWidgetManager, appWidgetId, newOptions);
        }

        #endregion

        [Service]
        public class Dx2Service : Service
        {
            TimeChangedReceiver timeChangedReceiver;

            public Dx2Service()
            {
                // なんかコメントしても動いてるんやけど…
                //intentFilter.AddAction(Intent.ActionTimeTick);
                //intentFilter.AddAction(Intent.ActionTimeChanged);
                //intentFilter.AddAction(Intent.ActionTimezoneChanged);
            }

            public override void OnCreate()
            {
                base.OnCreate();

                timeChangedReceiver = new TimeChangedReceiver();

                // config 作れねーかなーという残骸
                //ISharedPreferences preferences =
                //    PreferenceManager.GetDefaultSharedPreferences(this);
                //timeChangedReceiver.MustShowNotify =
                //    preferences.GetBoolean("checkbox_notify", true);
                //timeChangedReceiver.MustShowToast =
                //    preferences.GetBoolean("checkbox_toast", true);
                //timeChangedReceiver.Character =
                //    (Characters)preferences.GetInt("entryvalues_list_characters", 1);

                // 内部で初期値 true にした
                //timeChangedReceiver.MustShowNotify = true;
                //timeChangedReceiver.MustShowToast = true;
            }

            //#pragma warning disable CS0672 // メンバーは古い形式のメンバーをオーバーライドします
            //            public override void OnStart(Intent intent, int startId)
            //#pragma warning restore CS0672 // メンバーは古い形式のメンバーをオーバーライドします
            //            {
            //                IntentFilter filterTimeTick =
            //                    new IntentFilter(Intent.ActionTimeTick);
            //                IntentFilter filterTimeChanged =
            //                    new IntentFilter(Intent.ActionTimeChanged);
            //                IntentFilter filterTimeZoneChanged =
            //                    new IntentFilter(Intent.ActionTimezoneChanged);

            //                RegisterReceiver(timeChangedReceiver, filterTimeTick);
            //                RegisterReceiver(timeChangedReceiver, filterTimeChanged);
            //                RegisterReceiver(timeChangedReceiver, filterTimeZoneChanged);
            //            }

            [Obsolete()]
            public override void OnStart(Intent intent, int startId)
            {
                IntentFilter filterTimeTick =
                    new IntentFilter(Intent.ActionTimeTick);
                IntentFilter filterTimeChanged =
                    new IntentFilter(Intent.ActionTimeChanged);
                IntentFilter filterTimeZoneChanged =
                    new IntentFilter(Intent.ActionTimezoneChanged);

                RegisterReceiver(timeChangedReceiver, filterTimeTick);
                RegisterReceiver(timeChangedReceiver, filterTimeChanged);
                RegisterReceiver(timeChangedReceiver, filterTimeZoneChanged);
            }

            public override IBinder OnBind(Intent intent)
            {
                return null;
            }

            public override void OnDestroy()
            {
                base.OnDestroy();
                UnregisterReceiver(timeChangedReceiver);
            }
        }
    }
}
