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
            return await _db.Lessons.ToListAsync();
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
        public async Task DeleteOldLessons()
        {
            var today = DateTime.Today;
            var oldLessons = await _db.Lessons
                .Where(l => l.Date < today)
                .ToListAsync();
            if (oldLessons.Any())
            {
                _db.Lessons.RemoveRange(oldLessons);
                await _db.SaveChangesAsync();
            }
        }
    }
}