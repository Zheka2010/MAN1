using MAN1.Models;

namespace MAN1.Services
{
    public interface INotificationService
    {
        Task ScheduleLessonNotification(Lesson lesson);
        void CancelLessonNotification(int lessonId);
    }
}
