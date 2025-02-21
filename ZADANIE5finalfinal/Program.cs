using Microsoft.EntityFrameworkCore;
using ZADANIE5finalfinal.Data;
using ZADANIE5finalfinal.Services;

namespace ZADANIE5finalfinal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(
                builder.Configuration.GetConnectionString("DeafultConnectionString")   
            ));
			builder.Services.AddTransient<CSVService>();
			builder.Services.AddTransient<XLSXService>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Test}/{action=Index}/{id?}");

            app.Run();
        }
    }
}