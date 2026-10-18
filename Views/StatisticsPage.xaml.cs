using MAN1.Services;
using System.Globalization;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace MAN1.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly LessonService _lessonService;
    private DateTime _month = DateTime.Today;

    public StatisticsPage(LessonService lessonService)
    {
        InitializeComponent();

        _lessonService = lessonService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadStatistics();
    }

    private async Task LoadStatistics()
    {
        var statistics = await _lessonService.GetMonthlyStatistics(
            _month.Year,
            _month.Month
        );

        MonthLabel.Text = _month.ToString(
            "MMMM yyyy",
            new CultureInfo("uk-UA")
        );

        LessonsCountLabel.Text = statistics.Count.ToString();

        int hours = statistics.TotalMinutes / 60;
        int minutes = statistics.TotalMinutes % 60;

        TotalTimeLabel.Text = $"{hours} год {minutes} хв";
        var values = await _lessonService.GetLessonsByDay(_month.Year,_month.Month);

        LessonsChart.Series =
        [
            new ColumnSeries<int>
            {
                Values=values
            }
        ];

        LessonsChart.XAxes =
        [
            new Axis
            {
                Labels=Enumerable.Range(1,values.Count).Select(x=>x.ToString()).ToArray()
            }
        ];

        LessonsChart.YAxes =
        [
            new Axis
            {
                MinLimit=0,
                MinStep=1
            }
        ];
    }

    private async void PreviousMonth_Clicked(object sender, EventArgs e)
    {
        _month = _month.AddMonths(-1);
        await LoadStatistics();
    }

    private async void NextMonth_Clicked(object sender, EventArgs e)
    {
        _month = _month.AddMonths(1);
        await LoadStatistics();
    }

}