using Microsoft.EntityFrameworkCore;
using MAN1.Data;
using MAN1.Models;

namespace MAN1.Services
{
    public class LessonService
    {
        private readonly AppDbContext _db;
        public LessonService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Lesson>> GetLesson()
        {
            var lessons = await _db.Lessons
                .Where(x => !x.IsCompleted)
                .ToListAsync();

            return lessons
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartTime)
                .ToList();
        }
        public async Task AddLesson(Lesson lesson)
        {
            _db.Lessons.Add(lesson);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateLesson(Lesson lesson)
        {
            _db.Lessons.Update(lesson);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteLesson(Lesson lesson)
        {
            _db.Lessons.Remove(lesson);
            await _db.SaveChangesAsync();
        }
        public async Task CompleteOldLessons()
        {
            var today = DateTime.Today;
            var oldLessons = await _db.Lessons.Where(x => x.Date < today && !x.IsCompleted).ToListAsync();

            foreach(var lesson in oldLessons)
            {
                lesson.IsCompleted = true;
            }
            await _db.SaveChangesAsync();
        }
    }
}