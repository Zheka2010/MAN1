using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;


namespace MAN1.Views;

public partial class LessonsPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;
    private readonly INotificationService _notificationService;

    public LessonsPage(LessonsViewModel viewModel, INotificationService notificationService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _notificationService = notificationService;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.PostNotifications>();
        }
#endif

        await _viewModel.CompleteOldLessons();
        await _viewModel.LoadLesson();

    }

    private async void AddLessonButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddLessonPage(_viewModel, _notificationService));
    }

    private async void DeleteLessonButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Lesson lesson)
        {
            _notificationService.CancelLessonNotification(lesson.Id);

            await _viewModel.DeleteLesson(lesson);
        }
    }
}