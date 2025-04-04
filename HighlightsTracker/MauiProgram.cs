﻿using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using HighlightsTracker.ViewModels;
using Microsoft.Extensions.Logging;

namespace HighlightsTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<PeopleViewModel>();
            builder.Services.AddSingleton<ManagePeoplePage>();

            return builder.Build();
        }
    }
}
