using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Net;
using MAN1.Models;
using MAN1.Services;

namespace MAN1.Platforms.Android;

public class AndroidNotificationService : INotificationService
{
    public const string ChannelId = "lesson_notifications";

    public AndroidNotificationService()
    {
        CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                ChannelId,
                "Нагадування про заняття",
                NotificationImportance.High
            );

            channel.Description = "Сповіщення за 15 хвилин до заняття";

            var manager = (NotificationManager)
                Platform.AppContext.GetSystemService(
                    Context.NotificationService
                )!;

            manager.CreateNotificationChannel(channel);
        }
    }

    public Task ScheduleLessonNotification(Lesson lesson)
    {
        
        var lessonTime = lesson.Date.Date + lesson.StartTime;
        var notifyTime = lessonTime.AddMinutes(-15);

        if (notifyTime <= DateTime.Now)
        {
            return Task.CompletedTask;
        }

        var context = Platform.AppContext;

        var intent = new Intent(
            context,
            typeof(AlarmReceiver)
        );

        intent.PutExtra("id", lesson.Id);
        intent.PutExtra("subject", lesson.Subject);
        intent.PutExtra("student", lesson.StudentName);

        var pendingIntent = PendingIntent.GetBroadcast(
            context,
            lesson.Id,
            intent,
            PendingIntentFlags.UpdateCurrent |
            PendingIntentFlags.Immutable
        );

        var alarmManager = (AlarmManager)
            context.GetSystemService(Context.AlarmService)!;

        long milliseconds = new DateTimeOffset(notifyTime)
            .ToUnixTimeMilliseconds();

        if (Build.VERSION.SdkInt >= BuildVersionCodes.S && !alarmManager.CanScheduleExactAlarms())
        {
            alarmManager.SetAndAllowWhileIdle(
                AlarmType.RtcWakeup,
                milliseconds,
                pendingIntent
            );
        }
        else
        {
            alarmManager.SetExactAndAllowWhileIdle(
                AlarmType.RtcWakeup,
                milliseconds,
                pendingIntent
            );
        }

        return Task.CompletedTask;
    }

    public void CancelLessonNotification(int lessonId)
    {
        var context = Platform.AppContext;

        var intent = new Intent(
            context,
            typeof(AlarmReceiver)
        );

        var pendingIntent = PendingIntent.GetBroadcast(
            context,
            lessonId,
            intent,
            PendingIntentFlags.UpdateCurrent |
            PendingIntentFlags.Immutable
        );

        var alarmManager = (AlarmManager)
            context.GetSystemService(Context.AlarmService)!;

        alarmManager.Cancel(pendingIntent);

        pendingIntent.Cancel();
    }

    public bool CanScheduleExactAlarms()
    {
        var context = Platform.AppContext;

        var alarmManager = (AlarmManager)
            context.GetSystemService(Context.AlarmService)!;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
        {
            return alarmManager.CanScheduleExactAlarms();
        }

        return true;
    }

    public void RequestExactAlarmPermission()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
        {
            var context = Platform.AppContext;

            var alarmManager = (AlarmManager)
                context.GetSystemService(Context.AlarmService)!;

            if (!alarmManager.CanScheduleExactAlarms())
            {
                var intent = new Intent(
                    Settings.ActionRequestScheduleExactAlarm
                );

                intent.SetData(
                    global::Android.Net.Uri.Parse(
                    $"package:{context.PackageName}"
                    )
                );

                intent.AddFlags(
                    ActivityFlags.NewTask
                );

                context.StartActivity(intent);
            }
        }
    }
}