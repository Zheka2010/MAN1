using MAN1.Data;
using MAN1.Models;
using MAN1.Views;

namespace MAN1
{
    public partial class App : Application
    {
        public App(AppDbContext db,LessonsPage lessonsPage)
        {
            InitializeComponent();
            db.Database.EnsureCreated();

            MainPage = new NavigationPage(lessonsPage);
        }
        
    }
}