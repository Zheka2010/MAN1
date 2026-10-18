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

        public async Task<(int Count, int TotalMinutes)> GetMonthlyStatistics(int year,int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var lessons = await _db.Lessons.Where(x => x.IsCompleted &&x.Date >= start &&x.Date < end).ToListAsync();

            return (lessons.Count,lessons.Sum(x => x.Duration));
        }
        public async Task<List<int>> GetLessonsByDay(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var lessons = await _db.Lessons.Where(x => x.IsCompleted && x.Date >= start && x.Date < end).ToListAsync();

            int days = DateTime.DaysInMonth(year, month);

            var result = new List<int>();

            for (int day = 1; day <= days; day++)
            {
                result.Add(lessons.Count(x => x.Date.Day == day));
            }

            return result;
        }
    }
}