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
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var lessons = await _db.Lessons.Where(x => !x.IsCompleted && x.Date >= today && x.Date < tomorrow).ToListAsync();
            return lessons.OrderBy(x => x.StartTime).ToList();
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

        public async Task<(int Count, int TotalMinutes)> GetMonthlyStatistics(int year, int month, int? studentId = null)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var query = _db.Lessons.Where(x => x.IsCompleted && x.Date >= start && x.Date < end);
            if (studentId.HasValue && studentId.Value > 0)
            {
                query = query.Where(x => x.StudentId == studentId.Value);
            }

            var lessons = await query.ToListAsync();
            return (lessons.Count, lessons.Sum(x => x.Duration));
        }

        public async Task<List<int>> GetLessonsByDay(int year, int month, int? studentId = null)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var query = _db.Lessons.Where(x => x.IsCompleted && x.Date >= start && x.Date < end);
            if (studentId.HasValue && studentId.Value > 0)
            {
                query = query.Where(x => x.StudentId == studentId.Value);
            }

            var lessons = await query.ToListAsync();
            int days = DateTime.DaysInMonth(year, month);
            var result = new List<int>();

            for (int day = 1; day <= days; day++)
            {
                result.Add(lessons.Count(x => x.Date.Day == day));
            }

            return result;
        }
        public async Task<List<Lesson>> GetLessonsByDate(DateTime date)
        {
            var nextDay = date.Date.AddDays(1);
            return await _db.Lessons.Where(x => x.Date >= date.Date && x.Date < nextDay).ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsForMonth(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            var lessons = await _db.Lessons.Where(x => x.Date >= start && x.Date < end).ToListAsync();
            return lessons;
        }
        public class StudentStatItem
        {
            public string StudentName { get; set; } = "";
            public int Count { get; set; }
            public string TotalTime { get; set; } = "";
        }
        public async Task<List<StudentStatItem>> GetStatisticsByStudent(int year,int month)
        {
            var start = new DateTime(year,month,1);
            var end = start.AddMonths(1);
            var lessons = await _db.Lessons.Where(x => x.IsCompleted && x.Date >= start && x.Date < end).ToListAsync();
            return lessons.GroupBy(x => new { x.StudentId, x.StudentName }).Select(g => new StudentStatItem
            {
                StudentName = g.Key.StudentName,
                Count = g.Count(),
                TotalTime = $"{g.Sum(x => x.Duration) / 60} год {g.Sum(x => x.Duration) % 60} хв"
            })
            .OrderByDescending(x => x.Count).ToList();
        }
    }
}