using Blog_final_project.Data;
using Blog_final_project.Interfaces;
using Blog_final_project.Repositoties;
using Microsoft.EntityFrameworkCore;

namespace Blog_final_project;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlite("Data Source=blog.db"));

        builder.Services.AddAuthentication("MyCookieAuth")
            .AddCookie("MyCookieAuth", options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Error/AccessDenied";
            });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error/ServerError");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStatusCodePagesWithReExecute("/Error/NotFound");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
            await SeedData.InitializeAsync(context);
        }

        app.Run();
    }
}
