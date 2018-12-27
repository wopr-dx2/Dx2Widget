using System;
using Android;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace Dx2Widget
{
    class MoonAge
    {
        /// <summary>
        /// 満月開始から次の満月までの周期
        /// </summary>
        const int INTERVAL_MINUTES = 118;

        /// <summary>
        /// 満月と新月は 10 分間
        /// </summary>
        const int TEN_MINUTES = 10;

        /// <summary>
        /// 満月と新月以外は 7 分間
        /// </summary>
        const int SEVEN_MINUTES = 7;

        /// <summary>
        /// 5 分前にメッセージを表示する
        /// </summary>
        const int MESSAGE_MINUTES = -5;     // 5 分前

        /// <summary>
        /// 1 分前に再度メッセージを表示する
        /// </summary>
        const int SNOOZE_MINUTES = -1;      // 1 分前

        /// <summary>
        /// 標準時からの日本の時差
        /// </summary>
        readonly static TimeSpan jpTimeSpan = new TimeSpan(9, 0, 0);

        /// <summary>
        /// 満月基準日を 2018/08/01 01:55 とする
        /// </summary>
        /// <remarks>https://wikiwiki.jp/d2-megaten-l/アウラゲート</remarks>
        readonly DateTime JP_FULLMOON_REFER =
            new DateTimeOffset(2018, 8, 1, 1, 55, 0, jpTimeSpan).DateTime;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="dateTime">日時</param>
        public void Initialize(DateTime dateTime)
        {
            // 内部では日本時間で計算
            now = Local2Jp(dateTime);

            // 現在時刻との差を求める
            TimeSpan span = now.Subtract(JP_FULLMOON_REFER);
            // 分に換算
            double min = span.TotalMinutes;
            // 118 で割って切り捨て
            int div = (int)Math.Floor(min / INTERVAL_MINUTES);
            // 最終満月を求める
            LastFullMoon = new DateTime(JP_FULLMOON_REFER.AddMinutes(INTERVAL_MINUTES * div).Ticks);
            // 向こう 6 回分の満月を計算する（無駄打ちか？）
            CalcList(6);
        }

        /// <summary>
        /// 月齢の計算
        /// </summary>
        void CalcMoon()
        {
            // 最終満月 + 118 分を越えたら 1 周したことになる
            if (LastFullMoon.AddMinutes(INTERVAL_MINUTES) <= Now)
            {
                LastFullMoon = LastFullMoon.AddMinutes(INTERVAL_MINUTES);
            }

            #region 月齢 判定（ダサいしｗ）

            if (Now < LastFullMoon.AddMinutes(TEN_MINUTES))
            {
                Age = MoonAges.Full;
                MoonId = Resource.Drawable.FullMoon;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES))
            {
                Age = MoonAges.F7N;
                MoonId = Resource.Drawable.F7N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 2))
            {
                Age = MoonAges.F6N;
                MoonId = Resource.Drawable.F6N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 3))
            {
                Age = MoonAges.F5N;
                MoonId = Resource.Drawable.F5N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 4))
            {
                Age = MoonAges.F4N;
                MoonId = Resource.Drawable.F4N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 5))
            {
                Age = MoonAges.F3N;
                MoonId = Resource.Drawable.F3N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 6))
            {
                Age = MoonAges.F2N;
                MoonId = Resource.Drawable.F2N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7))
            {
                Age = MoonAges.F1N;
                MoonId = Resource.Drawable.F1N;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES))
            {
                Age = MoonAges.New;
                MoonId = Resource.Drawable.NewMoon;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES))
            {
                Age = MoonAges.N1F;
                MoonId = Resource.Drawable.N1F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 2))
            {
                Age = MoonAges.N2F;
                MoonId = Resource.Drawable.N2F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 3))
            {
                Age = MoonAges.N3F;
                MoonId = Resource.Drawable.N3F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 4))
            {
                Age = MoonAges.N4F;
                MoonId = Resource.Drawable.N4F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 5))
            {
                Age = MoonAges.N5F;
                MoonId = Resource.Drawable.N5F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 6))
            {
                Age = MoonAges.N6F;
                MoonId = Resource.Drawable.N6F;
            }
            else if (Now < LastFullMoon.AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7)
                .AddMinutes(TEN_MINUTES).AddMinutes(SEVEN_MINUTES * 7))
            {
                Age = MoonAges.N7F;
                MoonId = Resource.Drawable.N7F;
            }
            else
            {
                Age = MoonAges.none;    // エラーのような気がするけどなぁ
                MoonId = Resource.Drawable.Tetregrammaton;
            }

            #endregion
        }

        /// <summary>
        /// 5 分前、1 分前、終了を判定する
        /// </summary>
        void CalcMessage()
        {
            string hhmm = "yyyyMMddHHmm";

            // 5 分前及び 1 分前の判定

            DateTime before5min =
                LastFullMoon
                    .AddMinutes(INTERVAL_MINUTES)
                    .AddMinutes(MESSAGE_MINUTES);

            IsBefore5min =
                Now.ToString(hhmm) == before5min.ToString(hhmm);

            DateTime before1min =
                LastFullMoon
                    .AddMinutes(INTERVAL_MINUTES)
                    .AddMinutes(SNOOZE_MINUTES);

            IsBefore1min =
                Now.ToString(hhmm) == before1min.ToString(hhmm);

            // 終了の判定

            DateTime ended =
                LastFullMoon
                    .AddMinutes(TEN_MINUTES);

            IsFullmoonEnded =
                Now.ToString(hhmm) == ended.ToString(hhmm);
        }

        /// <summary>
        /// リスト（下部 6 行）の計算
        /// </summary>
        /// <param name="count">計算する数</param>
        void CalcList(int count)
        {
            FullMoonList = new DateTime[count];

            for (int i = 0; i < count; i++)
            {
                // 118 分ずつ足した値を入れておく
                FullMoonList[i] = new DateTime(LastFullMoon.AddMinutes(INTERVAL_MINUTES * (i + 1)).Ticks);
            }
        }

        /// <summary>
        /// ローカル時間から日本時間への変換
        /// </summary>
        /// <param name="dateTime">ローカル時間</param>
        /// <returns>日本時間</returns>
        DateTime Local2Jp(DateTime dateTime)
        {
            // 標準時を求める
            DateTimeOffset utc = new DateTimeOffset(dateTime).ToUniversalTime();
            // 日本時間（+9 hour）を足す
            DateTimeOffset jp = utc.Add(new TimeSpan(9, 0, 0));
            // 日本時間で返す
            return jp.DateTime;
        }

        private DateTime lastFullMoon;
        /// <summary>
        /// 前回の満月
        /// </summary>
        public DateTime LastFullMoon
        {
            get { return lastFullMoon; }
            set
            {
                if (lastFullMoon != value)
                {
                    // リスト作成
                    CalcList(6);
                }
                // 変化があるか
                IsLastFullMoonChanged = lastFullMoon != value;
                lastFullMoon = value;
            }
        }

        /// <summary>
        /// 次の満月
        /// </summary>
        public DateTime NextFullMoon =>
            LastFullMoon.AddMinutes(INTERVAL_MINUTES);

        /// <summary>
        /// 次の満月までの
        /// </summary>
        public int RemindMinutes =>
            (int)(NextFullMoon - Now).TotalMinutes + 1;

        /// <summary>
        /// 満月の予定リスト
        /// </summary>
        public DateTime[] FullMoonList { get; set; }

        private MoonAges age = MoonAges.none;
        /// <summary>
        /// 月齢
        /// </summary>
        public MoonAges Age
        {
            get { return age; }
            set
            {
                IsMoonAgeChanged = age != value;
                age = value;
            }
        }

        /// <summary>
        /// Resource.Drawable 値
        /// </summary>
        public int MoonId { get; private set; }

        private DateTime now;
        /// <summary>
        /// 現在時刻（日本時間）
        /// </summary>
        public DateTime Now
        {
            get { return now; }
            set
            {
                // システムで時刻合わせをすると盤面とかメッセージのタイミングが狂う
                // プログラム側では 1 分単位で変化してるはずなので
                // 急に 3 分以上の変化があったら初期化する
                if (Math.Abs(now.Subtract(value).TotalMinutes) > 3)
                {
                    Initialize(value);
                }
                else
                {
                    now = Local2Jp(value);
                }
                CalcMoon();
                CalcMessage();
            }
        }

        /// <summary>
        /// 満月が更新されたか
        /// </summary>
        public bool IsLastFullMoonChanged { get; private set; }

        /// <summary>
        /// 月齢が変更されたか
        /// </summary>
        public bool IsMoonAgeChanged { get; private set; }

        /// <summary>
        /// 満月の 5 分前か
        /// </summary>
        public bool IsBefore5min { get; private set; }

        /// <summary>
        /// 満月の 1 分前か
        /// </summary>
        public bool IsBefore1min { get; private set; }

        /// <summary>
        /// 満月が終了したか
        /// </summary>
        public bool IsFullmoonEnded { get; private set; }
    }
}