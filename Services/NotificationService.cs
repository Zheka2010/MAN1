using MAN1.Models;
using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;

namespace MAN1.Services
{
    public class NotificationService
    {
        public async Task ScheduleLessonNotification(Lesson lesson)
        {
            var lessonDateTime = lesson.Date.Date + lesson.StartTime;
            var notificationTime = lessonDateTime.AddMinutes(-15);

            if (notificationTime <= DateTime.Now)
            {
                return;
            }

            var request = new NotificationRequest
            {
                NotificationId = lesson.Id,
                Title = "Скоро заняття",
                Description = $"{lesson.Subject} з {lesson.StudentName} через 15 хвилин",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notificationTime
                }
            };

            await LocalNotificationCenter.Current.Show(request);
        }

        public void CancelLessonNotification(Lesson lesson)
        {
            LocalNotificationCenter.Current.Cancel(lesson.Id);
        }
    }
}
