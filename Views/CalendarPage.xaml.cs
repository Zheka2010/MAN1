using MAN1.Models;
using MAN1.Services;
using System.Globalization;

namespace MAN1.Views;

public partial class CalendarPage : ContentPage
{
    private readonly LessonService _lessonService;
    private DateTime _currentMonth = DateTime.Today;
    private DateTime _selectedDate = DateTime.Today;
    private List<Lesson> _monthLessons = new();

    public CalendarPage(LessonService lessonService)
    {
        InitializeComponent();
        _lessonService = lessonService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await LoadCalendar();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.ToString(), "OK");
        }
    }

    private async Task LoadCalendar()
    {
        _monthLessons = await _lessonService.GetLessonsForMonth(_currentMonth.Year, _currentMonth.Month);
        BuildCalendar();
        LoadLessonsForDate(_selectedDate);
    }
    private void BuildCalendar()
    {
        CalendarGrid.Children.Clear();
        MonthLabel.Text = _currentMonth.ToString("MMMM yyyy", new CultureInfo("uk-UA"));
        var firstDay = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);
        int startColumn = ((int)firstDay.DayOfWeek + 6) % 7;
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        for (int day = 1; day <= daysInMonth; day++)
        {
            int position = startColumn + day - 1;
            int row = position / 7;
            int column = position % 7;
            var date = new DateTime(_currentMonth.Year, _currentMonth.Month, day);
            bool isSelected = date.Date == _selectedDate.Date;
            bool isToday = date.Date == DateTime.Today;
            bool hasLessons = _monthLessons.Any(x => x.Date.Date == date.Date);
            var button = new Button
            {
                Text = day.ToString(),
                Padding = 0,
                HeightRequest = 45,
                CornerRadius = 22,
                CommandParameter = date,
                BackgroundColor = isSelected ? Color.FromArgb("#6750A4") : Colors.Transparent,
                TextColor = isSelected ? Colors.White : isDark ? Colors.White : Colors.Black
            };
            if (isToday)
            {
                button.BorderWidth = 2;
                button.BorderColor = Color.FromArgb("#6750A4");
            }
            button.Clicked += DayButton_Clicked;
            var dayGrid = new Grid();
            dayGrid.Children.Add(button);
            if (hasLessons)
            {
                var dot = new BoxView
                {
                    WidthRequest = 6,
                    HeightRequest = 6,
                    CornerRadius = 3,
                    BackgroundColor = isSelected ? Colors.White : Color.FromArgb("#6750A4"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    Margin = new Thickness(0, 0, 0, 3),
                    InputTransparent = true
                };
                dayGrid.Children.Add(dot);
            }
            CalendarGrid.Add(dayGrid, column, row);
        }
    }
    private void DayButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not DateTime date)return;
        _selectedDate = date;
        BuildCalendar();
        LoadLessonsForDate(date);
    }
    private void LoadLessonsForDate(DateTime date)
    {
        SelectedDateLabel.Text = date.ToString("'Заняття на' d MMMM yyyy", new CultureInfo("uk-UA"));
        LessonsCollection.ItemsSource = _monthLessons.Where(x => x.Date.Date == date.Date).OrderBy(x => x.StartTime).ToList();
    }
    private async void PreviousMonth_Clicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(-1);
        _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        await LoadCalendar();
    }
    private async void NextMonth_Clicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(1);
        _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        await LoadCalendar();
    }
    private async void DeleteLessonButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not Lesson lesson) return;

        await _lessonService.DeleteLesson(lesson);
        await LoadCalendar();
    }
}