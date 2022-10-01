using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;

internal class Program
{
    public static IConfiguration Configuration { get; set; } = null!;

    public static IEnumerable<VGrade> Grades { get; set; } = null!;

    public Program() => _ = LoadGradesAsync();

    private static async Task LoadGradesAsync() => Grades = await StudentService.GetGrades();

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        Configuration = builder.Configuration;
        // Add services to the container.
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}