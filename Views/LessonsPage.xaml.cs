using MAN1.ViewModel;

namespace MAN1.Views;

public partial class LessonsPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;

    public LessonsPage(LessonsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadLesson();
    }
}