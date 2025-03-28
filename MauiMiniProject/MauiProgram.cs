﻿using Microsoft.Extensions.Logging;
using MauiMiniProject.Services;
using MauiMiniProject.ViewModel;
using MauiMiniProject.Pages;

namespace MauiMiniProject;

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

        // ลงทะเบียน DataService เป็น Iservice
        builder.Services.AddSingleton<Iservice, DataService>();

        // ลงทะเบียน ViewModel
        builder.Services.AddSingleton<SearchViewModel>();
        builder.Services.AddSingleton<WithdrawViewModel>(); // ลงทะเบียน WithdrawViewModel
        builder.Services.AddSingleton<ViewCoursesViewModel>(); // ลงทะเบียน ViewCoursesViewModel

        // ลงทะเบียน Pages
        builder.Services.AddSingleton<ViewCoursesPage>(); // ใช้ singleton เพื่อให้แชร์กันทั้งแอป
        builder.Services.AddTransient<WithdrawCoursePage>(); // ใช้ transient เพื่อสร้างใหม่ทุกครั้งที่เรียกใช้

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
