using MAN1.Models;
using MAN1.Services;
using MAN1.ViewModel;


namespace MAN1.Views;

public partial class LessonsPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;
    private readonly INotificationService _notificationService;
    private readonly LessonService _lessonService;

    public LessonsPage(LessonsViewModel viewModel, INotificationService notificationService, LessonService lessonService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _notificationService = notificationService;
        BindingContext = _viewModel;
        _lessonService = lessonService;
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
    private async void LessonSelectionChanged(object sender,SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Lesson lesson)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Navigation.PushAsync(new EditLessonPage(lesson, _viewModel,_notificationService));
    }
    private async void StatisticsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new StatisticsPage(_lessonService)
        );
    }
}