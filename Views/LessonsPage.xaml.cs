using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;
using Plugin.LocalNotification;

namespace MAN1.Views;

public partial class LessonsPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;
    private readonly NotificationService _notificationService;

    public LessonsPage(LessonsViewModel viewModel, NotificationService notificationService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _notificationService = notificationService;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LocalNotificationCenter.Current.RequestNotificationPermission();

        await _viewModel.DeleteOldLessons();
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
            _notificationService.CancelLessonNotification(lesson);

            await _viewModel.DeleteLesson(lesson);
        }
    }
}