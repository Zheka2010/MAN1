using MAN1.Models;
using MAN1.Services;

namespace MAN1.Views;

public partial class StudentsPage : ContentPage
{
    private readonly StudentService _studentService;
    public StudentsPage(StudentService studentService)
    {
        InitializeComponent();
        _studentService = studentService;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadStudents();
    }
    private async Task LoadStudents()
    {
        StudentsCollection.ItemsSource = await _studentService.GetStudents();
    }

    private async void AddStudentButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddStudentPage(_studentService));
    }
    private async void DeleteStudentButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not Student student)return;
        await _studentService.DeleteStudent(student);
        await LoadStudents();
    }
}