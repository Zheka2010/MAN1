using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace MAN1.Platforms.Android;

[BroadcastReceiver(
    Enabled = true,
    Exported = false)]
public class AlarmReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        int id = intent.GetIntExtra("id", 0);

        string subject =
            intent.GetStringExtra("subject") ?? "Заняття";

        string student =
            intent.GetStringExtra("student") ?? "";

        var openAppIntent =
            new Intent(context, typeof(MainActivity));

        openAppIntent.SetFlags(
            ActivityFlags.ClearTop |
            ActivityFlags.SingleTop);

        var pendingIntent = PendingIntent.GetActivity(
            context,
            id,
            openAppIntent,
            PendingIntentFlags.Immutable |
            PendingIntentFlags.UpdateCurrent);

        var notification = new NotificationCompat.Builder(
                context,
                AndroidNotificationService.ChannelId)
            .SetContentTitle("Скоро заняття")
            .SetContentText($"{subject} з {student} через 15 хвилин")
            .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetAutoCancel(true)
            .SetContentIntent(pendingIntent)
            .Build();

        NotificationManagerCompat
            .From(context)
            .Notify(id, notification);
    }
}