using Microsoft.Extensions.Logging;
using Firebase.Database;
using Firebase.Database.Query;
using MauiAppMVVM.Models;
using MauiAppMVVM.ModelView;


namespace MauiAppMVVM
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif            
            return builder.Build();
        }

   
    }
}
