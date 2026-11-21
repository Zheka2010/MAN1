namespace MAN1.Views;

public partial class RegisterPage : ContentPage
{
    private readonly LessonsPage _lessonsPage;
    public RegisterPage(LessonsPage lessonsPage)
    {
        InitializeComponent();
        _lessonsPage = lessonsPage;
    }
    private async void RegisterButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Помилка", "Заповніть ім'я та пароль", "OK");
            return;
        }
        Preferences.Set("IsRegistered", true);
        Preferences.Set("TutorName", NameEntry.Text.Trim());
        Application.Current!.MainPage = new NavigationPage(_lessonsPage);
    }
}