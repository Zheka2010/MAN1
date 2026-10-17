using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;
using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;

namespace MAN1.Views;

public partial class AddLessonPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;
    private readonly NotificationService _notificationService;

    public AddLessonPage(LessonsViewModel viewModel, NotificationService notificationService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _notificationService = notificationService;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(StudentEntry.Text) ||
           string.IsNullOrWhiteSpace(SubjectEntry.Text))
        {
            await DisplayAlert("Помилка", "Заповніть ім'я учня і предмет", "OK");
            return;
        }

        var date = LessonDatePicker.Date ?? DateTime.Today;
        var time = LessonTimePicker.Time ?? TimeSpan.Zero;
        var lessonDateTime = date.Date + time;

        if(lessonDateTime < DateTime.Now)
        {
            await DisplayAlert("Помилка", "Дата та час уроку не можуть бути в минулому", "OK");
            return;
        }

        int duration = 60;

        if (int.TryParse(DurationEntry.Text, out int value))
        {
            duration = value;
        }

        var lesson = new Lesson
        {
            StudentName = StudentEntry.Text,
            Subject = SubjectEntry.Text,
            Date = LessonDatePicker.Date ?? DateTime.Today,
            StartTime = LessonTimePicker.Time ?? TimeSpan.Zero,
            Duration = duration,
            Comment = CommentEditor.Text ?? ""
        };

        await _viewModel.AddLesson(lesson);

        await _notificationService.ScheduleLessonNotification(lesson);

        await Navigation.PopAsync();
    }
}