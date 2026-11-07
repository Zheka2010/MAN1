using MAN1.Data;
using MAN1.Models;
using Microsoft.EntityFrameworkCore;

namespace MAN1.Services;

public class StudentService
{
    private readonly AppDbContext _db;
    public StudentService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<Student>> GetStudents()
    {
        return await _db.Students.OrderBy(x=>x.Name).ToListAsync();
    }
    public async Task AddStudent(Student student)
    {
        _db.Students.Add(student);
        await _db.SaveChangesAsync();
    }
    public async Task DeleteStudent(Student student)
    {
        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
    }
}
