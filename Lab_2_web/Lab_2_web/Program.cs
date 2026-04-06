using Microsoft.EntityFrameworkCore;

namespace Lab_2_web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Lab_2_web.Data.AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<Lab_2_web.Repositories.IBookingRepository, Lab_2_web.Repositories.BookingRepository>();
            builder.Services.AddScoped<Lab_2_web.Repositories.IRoomRepository,    Lab_2_web.Repositories.RoomRepository>();
            builder.Services.AddScoped<Lab_2_web.Services.BookingService>();
            builder.Services.AddScoped<Lab_2_web.Services.RoomService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Booking}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
