using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;

namespace MAN1.Views;

public partial class EditLessonPage : ContentPage
{
    private readonly Lesson _lesson;
    private readonly LessonsViewModel _viewModel;
    private readonly INotificationService _notificationService;

    public EditLessonPage(
        Lesson lesson,
        LessonsViewModel viewModel,
        INotificationService notificationService)
    {
        InitializeComponent();

        _lesson = lesson;
        _viewModel = viewModel;
        _notificationService = notificationService;

        StudentEntry.Text = lesson.StudentName;
        SubjectEntry.Text = lesson.Subject;
        LessonDatePicker.Date = lesson.Date;
        LessonTimePicker.Time = lesson.StartTime;
        DurationEntry.Text = lesson.Duration.ToString();
        CommentEditor.Text = lesson.Comment;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(StudentEntry.Text) || string.IsNullOrWhiteSpace(SubjectEntry.Text))
        {
            await DisplayAlert("Помилка","Заповніть ім'я учня і предмет","OK");

            return;
        }

        var date = LessonDatePicker.Date ?? DateTime.Today;
        var time = LessonTimePicker.Time ?? TimeSpan.Zero;

        var lessonDateTime = date.Date + time;

        if (lessonDateTime < DateTime.Now)
        {
            await DisplayAlert("Помилка","Дата та час не можуть бути в минулому","OK");

            return;
        }

        int duration = 60;

        if (int.TryParse(DurationEntry.Text, out int value))
        {
            duration = value;
        }

        _notificationService.CancelLessonNotification(_lesson.Id);

        _lesson.StudentName = StudentEntry.Text;
        _lesson.Subject = SubjectEntry.Text;
        _lesson.Date = date;
        _lesson.StartTime = time;
        _lesson.Duration = duration;
        _lesson.Comment = CommentEditor.Text ?? "";

        await _viewModel.UpdateLesson(_lesson);

        await _notificationService.ScheduleLessonNotification(_lesson);

        await Navigation.PopAsync();
    }
}