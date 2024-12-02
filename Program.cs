using Queuing_System.Helpers;
using Queuing_System.Services;

namespace Queuing_System
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services Add services to the DI container
			// Add services to the container.
			builder.Services.AddScoped<IQueueCalculator, QueueCalculator>(); // Register the interface and its implementaion in DI container
			builder.Services.AddControllersWithViews();
			builder.Services.AddTransient<PlotService>();
			#endregion

			var app = builder.Build();

			#region Configure - Configure the HTTP request pipeline.
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
				pattern: "{controller=Home}/{action=Index}/{id?}");

			#endregion
			app.Run();
		}
	}
}
