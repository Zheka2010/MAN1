using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;

namespace MAN1.Views;

public partial class AddLessonPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;
    private readonly INotificationService _notificationService;
    private readonly StudentService _studentService;

    public AddLessonPage(LessonsViewModel viewModel, INotificationService notificationService, StudentService studentService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _notificationService = notificationService;
        _studentService = studentService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        StudentPicker.ItemsSource = await _studentService.GetStudents();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (StudentPicker.SelectedItem is not Student student)
        {
            await DisplayAlert("Помилка", "Виберіть учня", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SubjectEntry.Text))
        {
            await DisplayAlert("Помилка", "Введіть предмет", "OK");
            return;
        }
        var date = LessonDatePicker.Date ?? DateTime.Today;
        var time = LessonTimePicker.Time ?? TimeSpan.Zero;
        var lessonDateTime = date.Date + time;
        if (lessonDateTime < DateTime.Now)
        {
            await DisplayAlert("Помилка", "Дата та час уроку не можуть бути в минулому", "OK");
            return;
        }
        int duration = 60;
        if (int.TryParse(DurationEntry.Text, out int value) && value>0)duration = value;
        if (RepeatWeeklySwitch.IsToggled)
        {
            var currentDate = date.Date;
            var endOfMonth = new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month));
            while (currentDate <= endOfMonth)
            {
                var lesson = new Lesson
                {
                    StudentId = student.Id,
                    StudentName = student.Name,
                    Subject = SubjectEntry.Text.Trim(),
                    Date = currentDate,
                    StartTime = time,
                    Duration = duration,
                    Comment = CommentEditor.Text?.Trim() ?? "",
                    IsCompleted = false
                };
                await _viewModel.AddLesson(lesson);
                await _notificationService.ScheduleLessonNotification(lesson);
                currentDate = currentDate.AddDays(7);
            }
        }
        else
        {
            var lesson = new Lesson
            {
                StudentId = student.Id,
                StudentName = student.Name,
                Subject = SubjectEntry.Text.Trim(),
                Date = date,
                StartTime = time,
                Duration = duration,
                Comment = CommentEditor.Text?.Trim() ?? "",
                IsCompleted = false
            };
            await _viewModel.AddLesson(lesson);
            await _notificationService.ScheduleLessonNotification(lesson);
        }
        await Navigation.PopAsync();
    }
}