using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace GuideApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseBarcodeReader();
            //                .ConfigureFonts(fonts =>
            //                {
            //                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            //                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            //                });

            //#if DEBUG
            //    		builder.Logging.AddDebug();
            //#endif

            return builder.Build();
        }
    }
}
