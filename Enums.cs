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
    /// <summary>
    /// 月齢
    /// </summary>
    public enum MoonAges
    {
        none,
        Full, F7N, F6N, F5N, F4N, F3N, F2N, F1N,
        New, N1F, N2F, N3F, N4F, N5F, N6F, N7F
    }

    // 未実装
    //public enum FaceStyles { Logo, Moon }
    //public enum NotifyStyles { Text, Image }

    /// <summary>
    /// キャラクタの選択
    /// </summary>
    public enum Characters { Player, TemplarDragon, Eileen, Shionyan, none }

    /// <summary>
    /// メッセージの選択
    /// </summary>
    public enum Messages { Before5min, Before1min, Ended, none }
}