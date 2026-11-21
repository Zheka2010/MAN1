using MAN1.Data;
using MAN1.Views;

namespace MAN1;

public partial class App : Application
{
    public App(AppDbContext db, LessonsPage lessonsPage)
    {
        InitializeComponent();

        db.Database.EnsureCreated();

        if (Preferences.Get("IsRegistered", false))
            MainPage = new NavigationPage(lessonsPage);
        else
            MainPage = new NavigationPage(new RegisterPage(lessonsPage));
    }
}