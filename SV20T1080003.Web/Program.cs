﻿using Microsoft.AspNetCore.Authentication.Cookies;
using SV20T1080003.Web;
using SV20T1080003.Web.AppCodes;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllersWithViews()
            .AddMvcOptions(option =>
            {
                option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //Không sử dụng thông báo mặc định cho giá trị null
            });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.Cookie.Name = "AuthenticationCookie";
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/AccessDenined";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });

        builder.Services.AddSession(option =>
        {
            option.IdleTimeout = TimeSpan.FromMinutes(60);
            option.Cookie.HttpOnly = true;
            option.Cookie.IsEssential = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapAreaControllerRoute(
                name: "areaAdmin",
                areaName: "Admin",
                pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        //Khởi tạo cấu hình cho ApplicationContext
        ApplicationContext.Configure
        (
            httpContextAccessor: app.Services.GetRequiredService<IHttpContextAccessor>(),
            hostEnvironment: app.Services.GetService<IWebHostEnvironment>()
        );

        app.Run();
    }
}