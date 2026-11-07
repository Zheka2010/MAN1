using MAN1.Models;
using MAN1.Services;

namespace MAN1.Views;

public partial class AddStudentPage : ContentPage
{
    private readonly StudentService _studentService;

    public AddStudentPage(StudentService studentService)
    {
        InitializeComponent();
        _studentService = studentService;
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Помилка", "Введіть ім'я учня", "OK");
            return;
        }
        var student = new Student
        {
            Name = NameEntry.Text.Trim()
        };
        await _studentService.AddStudent(student);
        await Navigation.PopAsync();
    }
}