using MAN1.Models;
using MAN1.Services;
using System.Globalization;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace MAN1.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly LessonService _lessonService;
    private readonly StudentService _studentService;
    private int? _selectedStudentId = null;
    private DateTime _month = DateTime.Today;

    public StatisticsPage(LessonService lessonService, StudentService studentService)
    {
        InitializeComponent();
        _lessonService = lessonService;
        _studentService = studentService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadStudentsPicker();
        await LoadStatistics();
    }

    private async Task LoadStudentsPicker()
    {
        try
        {
            var students = await _studentService.GetStudents();
            var list = new List<Student> { new Student { Id = 0, Name = "Усі учні" } };
            list.AddRange(students);

            StudentFilterPicker.ItemsSource = list;

            if (StudentFilterPicker.SelectedIndex == -1 && list.Count > 0)
            {
                StudentFilterPicker.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void StudentFilterPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (StudentFilterPicker.SelectedItem is Student student)
        {
            _selectedStudentId = student.Id > 0 ? student.Id : null;
        }
        else
        {
            _selectedStudentId = null;
        }

        await LoadStatistics();
    }

    private async Task LoadStatistics()
    {
        var statistics = await _lessonService.GetMonthlyStatistics(
            _month.Year,
            _month.Month,
            _selectedStudentId
        );

        MonthLabel.Text = _month.ToString(
            "MMMM yyyy",
            new CultureInfo("uk-UA")
        );

        LessonsCountLabel.Text = statistics.Count.ToString();

        int hours = statistics.TotalMinutes / 60;
        int minutes = statistics.TotalMinutes % 60;

        TotalTimeLabel.Text = $"{hours} год {minutes} хв";
        var values = await _lessonService.GetLessonsByDay(_month.Year, _month.Month, _selectedStudentId);

        LessonsChart.Series =
        [
            new ColumnSeries<int>
            {
                Values = values
            }
        ];

        LessonsChart.XAxes =
        [
            new Axis
            {
                Labels = Enumerable.Range(1, values.Count).Select(x => x.ToString()).ToArray()
            }
        ];

        LessonsChart.YAxes =
        [
            new Axis
            {
                MinLimit = 0,
                MinStep = 1
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