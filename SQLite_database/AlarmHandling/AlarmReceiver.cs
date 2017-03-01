using Android.App;
using Android.Widget;
using Android.OS;
using SQLite_database.Resources.DBHelper;
using System.Collections.Generic;
using SQLite_database.Model;
using Android.Util;
using System;
using Android.Content;
using Android.Graphics;
using Android;
using Android.Views;
using Android.Media;

namespace SQLite_database.AlarmHandling
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");

            var resultIntent = new Intent(context, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            var pending = PendingIntent.GetActivity(context, 0,
                resultIntent,
                PendingIntentFlags.CancelCurrent);

            Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            Int64[] pattern = { 500, 500, 500, 500, 500, 500, 500, 500, 500 };

            var builder =
                new Notification.Builder(context)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.Clock)
                    .SetDefaults(NotificationDefaults.All)
                    .SetTicker("Suave as fuck tho.")
                    .SetSound(alarmSound)
                    .SetVibrate(pattern)
                    .SetStyle(new Notification.InboxStyle())
                    .SetLights(Color.Blue, 500, 500)
                    .SetContentIntent(pending);

            var notification = builder.Build();

            var manager = NotificationManager.FromContext(context);
            manager.Notify(1337, notification);
        }
    }
}