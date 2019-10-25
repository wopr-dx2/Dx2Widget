using System;
using Android;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
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
    [BroadcastReceiver(Enabled = true, Exported = false)]
    class TimeChangedReceiver : BroadcastReceiver
    {
        /// <summary>
        /// 年月表示
        /// </summary>
        const string FORMAT_MMDD = "MM/dd";

        /// <summary>
        /// 時分表示
        /// </summary>
        const string FORMAT_HHMM = "HH:mm";

        /// <summary>
        /// 日付表示用 TextView Resource.Id 保管
        /// </summary>
        readonly int[] dateIds = new int[6]
        {
            Resource.Id.date1, Resource.Id.date2, Resource.Id.date3,
            Resource.Id.date4, Resource.Id.date5, Resource.Id.date6
        };

        /// <summary>
        /// 時分表示用 TextView Resource.Id 保管
        /// </summary>
        readonly int[] timeIds = new int[6]
        {
            Resource.Id.time1, Resource.Id.time2, Resource.Id.time3,
            Resource.Id.time4, Resource.Id.time5, Resource.Id.time6
        };

        /// <summary>
        /// リストグラフ表示用 ImageView Resource.Id 保管
        /// </summary>
        readonly int[] imageIds = new int[6]
        {
            Resource.Id.image1, Resource.Id.image2, Resource.Id.image3,
            Resource.Id.image4, Resource.Id.image5, Resource.Id.image6
        };

        /// <summary>
        /// リストグラフ表示用 Resource.Drawable 保管
        /// </summary>
        readonly int[] bitmapIds = new int[60]
        {
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,

            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.no_mente, Resource.Drawable.no_mente,
            Resource.Drawable.no_mente, Resource.Drawable.mente_46, Resource.Drawable.mente_47,
            Resource.Drawable.mente_48, Resource.Drawable.mente_49, Resource.Drawable.mente_50,
            Resource.Drawable.mente_51, Resource.Drawable.mente_52, Resource.Drawable.mente_53,
            Resource.Drawable.mente_54, Resource.Drawable.mente_55, Resource.Drawable.mente_56,
            Resource.Drawable.mente_57, Resource.Drawable.mente_58, Resource.Drawable.mente_59
        };

        /// <summary>
        /// リマインダバー表示用 Resource.Drawable 保管
        /// </summary>
        readonly int[] remindIds = new int[109]
        {
            Resource.Drawable.remind_000, Resource.Drawable.remind_001, Resource.Drawable.remind_002,
            Resource.Drawable.remind_003, Resource.Drawable.remind_004, Resource.Drawable.remind_005,
            Resource.Drawable.remind_006, Resource.Drawable.remind_007, Resource.Drawable.remind_008,
            Resource.Drawable.remind_009, Resource.Drawable.remind_010, Resource.Drawable.remind_011,
            Resource.Drawable.remind_012, Resource.Drawable.remind_013, Resource.Drawable.remind_014,
            Resource.Drawable.remind_015, Resource.Drawable.remind_016, Resource.Drawable.remind_017,
            Resource.Drawable.remind_018, Resource.Drawable.remind_019, Resource.Drawable.remind_020,
            Resource.Drawable.remind_021, Resource.Drawable.remind_022, Resource.Drawable.remind_023,
            Resource.Drawable.remind_024, Resource.Drawable.remind_025, Resource.Drawable.remind_026,
            Resource.Drawable.remind_027, Resource.Drawable.remind_028, Resource.Drawable.remind_029,
            Resource.Drawable.remind_030, Resource.Drawable.remind_031, Resource.Drawable.remind_032,
            Resource.Drawable.remind_033, Resource.Drawable.remind_034, Resource.Drawable.remind_035,
            Resource.Drawable.remind_036, Resource.Drawable.remind_037, Resource.Drawable.remind_038,
            Resource.Drawable.remind_039, Resource.Drawable.remind_040, Resource.Drawable.remind_041,
            Resource.Drawable.remind_042, Resource.Drawable.remind_043, Resource.Drawable.remind_044,
            Resource.Drawable.remind_045, Resource.Drawable.remind_046, Resource.Drawable.remind_047,
            Resource.Drawable.remind_048, Resource.Drawable.remind_049, Resource.Drawable.remind_050,
            Resource.Drawable.remind_051, Resource.Drawable.remind_052, Resource.Drawable.remind_053,
            Resource.Drawable.remind_054, Resource.Drawable.remind_055, Resource.Drawable.remind_056,
            Resource.Drawable.remind_057, Resource.Drawable.remind_058, Resource.Drawable.remind_059,
            Resource.Drawable.remind_060, Resource.Drawable.remind_061, Resource.Drawable.remind_062,
            Resource.Drawable.remind_063, Resource.Drawable.remind_064, Resource.Drawable.remind_065,
            Resource.Drawable.remind_066, Resource.Drawable.remind_067, Resource.Drawable.remind_068,
            Resource.Drawable.remind_069, Resource.Drawable.remind_070, Resource.Drawable.remind_071,
            Resource.Drawable.remind_072, Resource.Drawable.remind_073, Resource.Drawable.remind_074,
            Resource.Drawable.remind_075, Resource.Drawable.remind_076, Resource.Drawable.remind_077,
            Resource.Drawable.remind_078, Resource.Drawable.remind_079, Resource.Drawable.remind_080,
            Resource.Drawable.remind_081, Resource.Drawable.remind_082, Resource.Drawable.remind_083,
            Resource.Drawable.remind_084, Resource.Drawable.remind_085, Resource.Drawable.remind_086,
            Resource.Drawable.remind_087, Resource.Drawable.remind_088, Resource.Drawable.remind_089,
            Resource.Drawable.remind_090, Resource.Drawable.remind_091, Resource.Drawable.remind_092,
            Resource.Drawable.remind_093, Resource.Drawable.remind_094, Resource.Drawable.remind_095,
            Resource.Drawable.remind_096, Resource.Drawable.remind_097, Resource.Drawable.remind_098,
            Resource.Drawable.remind_099, Resource.Drawable.remind_100, Resource.Drawable.remind_101,
            Resource.Drawable.remind_102, Resource.Drawable.remind_103, Resource.Drawable.remind_104,
            Resource.Drawable.remind_105, Resource.Drawable.remind_106, Resource.Drawable.remind_107,
            Resource.Drawable.remind_108
        };

        public TimeChangedReceiver()
        {
            // 月齢計算クラスの初期化
            Moon = new MoonAge();
            Moon.Initialize(DateTime.Now);
        }

        /// <summary>
        /// いろいろなタイミングで呼び出されるみたい
        /// </summary>
        /// <param name="context">よく分からないが context が渡されます</param>
        /// <param name="intent">よく分からないが intent が渡されます</param>
        public override void OnReceive(Context context, Intent intent)
        {
            // intent.Action に、何の用事で呼び出されたかが入っているみたい
            string action = intent.Action;

            // 時間が変わった（1 分毎）、時間が変更された、タイムゾーンが変更された
            // 以外の内容でここに来た場合は処理終了
            if (!action.Equals(Intent.ActionTimeTick) &
                !action.Equals(Intent.ActionTimeChanged) &
                !action.Equals(Intent.ActionTimezoneChanged))
            {
                return;
            }

            // 月齢計算クラスに現在時刻を渡す
            Moon.Now = DateTime.Now;

            // 時差の計算
            // 満月の時間（日本時間）から 9 時間（日本標準時）を引いて、
            // ローカル offset 値を足すようです
            TimeSpan jp = new TimeSpan(-9, 0, 0);
            TimeSpan local = new DateTimeOffset(DateTime.Now).Offset;

            // 表示関係（TextView や ImageView など）を操作するときのおまじない
            ComponentName componentName =
                new ComponentName(context, Java.Lang.Class.FromType(typeof(Dx2WidgetProvider)));
            RemoteViews remoteViews =
                new RemoteViews(context.PackageName, Resource.Layout.widget_moon);

            // 55 分～ 59 分まではメンテなので曇り
            if (Moon.Now.Minute >= 55)
            {
                remoteViews.SetImageViewResource(Resource.Id.cloud, Resource.Drawable.Cloudy);
            }
            else
            {
                remoteViews.SetImageViewResource(Resource.Id.cloud, Resource.Drawable.Clear);
            }

            // 月齢の表示
            remoteViews.SetImageViewResource(Resource.Id.moon, Moon.MoonId);

            // 現在時間の表示  -- v0.9.3 時差対応
            remoteViews.SetTextViewText(
                Resource.Id.time0,
                Moon.Now.Add(jp).Add(local).ToString(FORMAT_HHMM));

            // リマインダバーと残り時間（分）の表示
            if (Moon.Age == MoonAges.Full)
            {
                // 満月の場合は、リマインダバーを黒にして残り時間を表示しない
                remoteViews.SetImageViewResource(
                    Resource.Id.reminder_bar, remindIds[0]);
                remoteViews.SetTextViewText(Resource.Id.reminder_time, "");
            }
            else
            {
                // 満月以外の場合のリマインダバーと残り時間（分）の表示
                int minutes = Moon.RemindMinutes;
                if (minutes >= 1 & minutes <= 108)
                {
                    remoteViews.SetImageViewResource(
                        Resource.Id.reminder_bar, remindIds[minutes]);
                }
                else
                {
                    remoteViews.SetImageViewResource(
                        Resource.Id.reminder_bar, remindIds[0]);
                }
                remoteViews.SetTextViewText(
                    Resource.Id.reminder_time, Moon.RemindMinutes.ToString());
            }

            //// 時差の関係
            //// 満月の時間（日本時間）から 9 時間（日本標準時）を引いて、
            //// ローカル offset 値を足すようです
            //TimeSpan jp = new TimeSpan(-9, 0, 0);
            //TimeSpan local = new DateTimeOffset(DateTime.Now).Offset;

            DateTime[] dateTimes = new DateTime[6]
            {
                Moon.FullMoonList[0].Add(jp).Add(local),
                Moon.FullMoonList[1].Add(jp).Add(local),
                Moon.FullMoonList[2].Add(jp).Add(local),
                Moon.FullMoonList[3].Add(jp).Add(local),
                Moon.FullMoonList[4].Add(jp).Add(local),
                Moon.FullMoonList[5].Add(jp).Add(local)
            };

            // 日付・時間・グラフの表示
            // 日付が上と同じ場合は非表示
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    remoteViews.SetTextViewText(dateIds[0], dateTimes[0].ToString(FORMAT_MMDD));
                }
                else if (dateTimes[i].Day == dateTimes[i - 1].Day)
                {
                    remoteViews.SetTextViewText(dateIds[i], "");
                }
                else
                {
                    remoteViews.SetTextViewText(dateIds[i], dateTimes[i].ToString(FORMAT_MMDD));
                }

                remoteViews.SetImageViewResource(
                    imageIds[i], bitmapIds[Moon.FullMoonList[i].Minute]);

                remoteViews.SetTextViewText(timeIds[i], dateTimes[i].ToString(FORMAT_HHMM));
            }

            // 画面の更新
            AppWidgetManager.GetInstance(context).UpdateAppWidget(componentName, remoteViews);

            #region 通知とトースターの処理

            // 通知のタイトル
            string title = "";
            // 通知の本文
            string text = "";
            // 何を通知するか
            Messages messages = Messages.none;
            // 次の満月の時間
            DateTime next = Moon.NextFullMoon.Add(jp).Add(local);

            if (Moon.IsBefore5min)
            {
                // 5 分前
                title = "満月 5 分前";
                text = string.Format("{0} 時 {1} 分から満月です", next.Hour, next.Minute);
                messages = Messages.Before5min;
            }
            else if (Moon.IsBefore1min)
            {
                // 1 分前
                title = "満月 1 分前";
                text = string.Format("{0} 時 {1} 分から満月です", next.Hour, next.Minute);
                messages = Messages.Before1min;
            }
            else if (Moon.IsFullmoonEnded)
            {
                // 終了
                title = "満月が終了しました";
                text = string.Format("次の満月は {0} 日 {1} 時 {2} 分からです", next.Day, next.Hour, next.Minute);
                messages = Messages.Ended;
            }

            // 何らかのメッセージ表示がある
            if (messages != Messages.none)
            {
                // 通知
                if (MustShowNotify)
                {
                    if (messages == Messages.Ended)
                    {
                        // 終了の通知
                        showNotify(context, title, text);
                    }
                    else
                    {
                        // 5 分前と 1 分前の通知は Dx2 起動ボタンを付ける
                        showButtonNotify(context, title, text);
                    }
                }

                // トースト
                if (MustShowToast)
                {
                    showToast(context, messages);
                }
            }

            #endregion
        }

        /// <summary>
        /// 通知の表示
        /// </summary>
        /// <param name="context">何か分からんけど context</param>
        /// <param name="title">通知タイトル</param>
        /// <param name="text">通知内容</param>
        /// <remarks>満月終了時</remarks>
        void showNotify(Context context, string title, string text)
        {
            // NotificationCompat.Builder 作成
            NotificationCompat.Builder builder =
                new NotificationCompat.Builder(context)
                .SetContentTitle(title)
                .SetContentText(text)
                .SetSmallIcon(Resource.Mipmap.ic_stat_icon)
                .SetDefaults((int)NotificationDefaults.Vibrate);    // 振動あり

            // notification に builder を Build() して渡す
            Notification notification = builder.Build();

            // manager に context を渡す
            NotificationManagerCompat notificationManager =
                NotificationManagerCompat.From(context);

            // manager で通知発信
            notificationManager.Notify(0, notification);
        }

        /// <summary>
        /// 通知の表示（ Dx2 起動ボタンあり）
        /// </summary>
        /// <param name="context">何か分からんけど context</param>
        /// <param name="title">通知タイトル</param>
        /// <param name="text">通知内容</param>
        /// <remarks>5 分前、1 分前</remarks>
        void showButtonNotify(Context context, string title, string text)
        {
            // intent に Dx2Megaten の Package Name と Class Name を指定します
            Intent intent = new Intent()
                .SetClassName(
                    "com.sega.d2megaten",
                    "com.sega.sgn.sgnfw.common.unityactivity.SgnfwUnityActivity");
            // で、なんか pendingIntent を作るみたいな感じ
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder =
                new NotificationCompat.Builder(context)
                .SetContentTitle(title)
                .SetContentText(text)
                .SetSmallIcon(Resource.Mipmap.ic_stat_icon)
                .AddAction(Resource.Drawable.Loki, "Dx2 を起動", pendingIntent)    // ← ここが違う
                .SetDefaults((int)NotificationDefaults.Vibrate);

            // ここからは showNotify と同じ
            Notification notification = builder.Build();

            NotificationManagerCompat notificationManager =
                NotificationManagerCompat.From(context);

            notificationManager.Notify(0, notification);
        }

        /// <summary>
        /// トーストの表示
        /// </summary>
        /// <param name="context">何か分からんけど context</param>
        /// <param name="messages">表示内容</param>
        void showToast(Context context, Messages messages)
        {
            // context から toast を作る
            Toast toast = new Toast(context);

            // context から inflater を作る
            var inflater = LayoutInflater.From(context);

            // inflater.Inflate に Layout を渡し、view を取得する
            View view = inflater.Inflate(Resource.Layout.char_toast, null);

            // 取得した view 内の ImageView（ balloon & character ）を取得する
            ImageView imageBalloon = (ImageView)view.FindViewById(Resource.Id.balloon);
            ImageView imageCharacter =
                (ImageView)view.FindViewById(Resource.Id.characters);

            // messages の内容で balloon & character の内容を変更する
            switch (messages)
            {
                case Messages.Before5min:
                    imageBalloon.SetImageResource(Resource.Drawable.BalloonBefore5min);
                    imageCharacter.SetImageResource(Resource.Drawable.CharTemplarDragon);
                    break;
                case Messages.Before1min:
                    imageBalloon.SetImageResource(Resource.Drawable.BalloonBefore1min);
                    imageCharacter.SetImageResource(Resource.Drawable.CharEileen);
                    break;
                case Messages.Ended:
                    imageBalloon.SetImageResource(Resource.Drawable.BalloonEnded);
                    imageCharacter.SetImageResource(Resource.Drawable.CharShionyan);
                    break;
            }

            // toast.View に balloon & character を変更した view を設定する
            toast.View = view;
            // 時間は長めに表示する
            toast.Duration = ToastLength.Long;
            // 表示位置は下の方
            toast.SetGravity(GravityFlags.Bottom, 0, 0);
            // toast の表示
            toast.Show();
        }

        /// <summary>
        /// 月齢
        /// </summary>
        public MoonAge Moon { get; set; }

        /// <summary>
        /// コンフィグで通知を表示するかどうか（default: true）
        /// </summary>
        public bool MustShowNotify { get; set; } = true;

        /// <summary>
        /// コンフィグでトースト表示するかどうか（ default: true ）
        /// </summary>
        public bool MustShowToast { get; set; } = true;

        // config でキャラクタ選択できるようにならんかと思ってた名残り
        //public Characters Character { get; set; } = Characters.Player;
    }
}