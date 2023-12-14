using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using University_System.Data;
using University_System.Reposibility;
using University_System.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UniDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UniversitySystem") ?? throw new InvalidOperationException("Connection string 'UniversitySystem' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//build service and repository
//Students
builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IStudentsReponsitory, StudentsRepo>();

//Teachers
builder.Services.AddScoped<ITeachersService, TeachersService>();
builder.Services.AddScoped<ITeachersReponsitory, TeachersRepo>();

//Courses
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<ICoursesReponsitory, CoursesRepo>();

//Score results
builder.Services.AddScoped<IScoreResultsService, ScoreResultsService>();
builder.Services.AddScoped<IScoreResultsReponsitory, ScoreResultsRepo>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
