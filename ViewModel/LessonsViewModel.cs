using System.Collections.ObjectModel;
using MAN1.Models;
using MAN1.Services;

namespace MAN1.ViewModel
{
    public class LessonsViewModel
    {
        private readonly LessonService _lessonService;
        public ObservableCollection<Lesson> Lessons { get; set; } = new();

        public LessonsViewModel(LessonService lessonService)
        {
            _lessonService = lessonService;
        }
        public async Task LoadLesson()
        {
            var lessons = await _lessonService.GetLesson();
            Lessons.Clear();
            foreach (var lesson in lessons)
            {
                Lessons.Add(lesson);
            }
        }
        public async Task AddLesson(Lesson lesson)
        {
            await _lessonService.AddLesson(lesson);
            Lessons.Add(lesson);
        }
        public async Task DeleteLesson(Lesson lesson)
        {
            await _lessonService.DeleteLesson(lesson);
            Lessons.Remove(lesson);
        }
    }
}
