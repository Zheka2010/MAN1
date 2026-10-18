namespace MAN1.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Duration { get; set; }
        public string Comment { get; set; } = "";
        public bool IsCompleted { get; set; }
    }
}
