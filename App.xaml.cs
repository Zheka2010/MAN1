using MAN1.Data;

namespace MAN1
{
    public partial class App : Application
    {
        public App(AppDbContext db)
        {
            InitializeComponent();
            db.Database.EnsureCreated();
            MainPage = new AppShell();
        }

        
    }
}