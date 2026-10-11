using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;

namespace MAN1.Views;

public partial class AddLessonPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;

    public AddLessonPage(LessonsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(StudentEntry.Text) ||
           string.IsNullOrWhiteSpace(SubjectEntry.Text))
        {
            await DisplayAlert("Помилка", "Заповніть ім'я учня і предмет", "OK");
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

        await Navigation.PopAsync();
    }
}