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
            NewProducto();
            return builder.Build();
        }

        public static void NewProducto()
        {
            FirebaseClient client = new FirebaseClient("https://mvvm-741a5-default-rtdb.firebaseio.com/");
            ProductosViewModels h = new ProductosViewModels();
            var productos = client.Child("Productos").OnceAsync<Productos>();
            
        }
    }
}
