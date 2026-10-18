using Microsoft.EntityFrameworkCore;
using MAN1.Data;
using MAN1.Services;
using MAN1.Views;
using MAN1.ViewModel;

namespace MAN1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "lessons.db");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
            builder.Services.AddTransient<LessonService>();
            builder.Services.AddTransient<LessonsViewModel>();
            builder.Services.AddTransient<LessonsPage>();
            builder.Services.AddTransient<AddLessonPage>();
            builder.Services.AddTransient<NotificationService>();
            #if ANDROID
            builder.Services.AddSingleton<INotificationService,MAN1.Platforms.Android.AndroidNotificationService>();
            #endif

            return builder.Build();
        }
    }
}
