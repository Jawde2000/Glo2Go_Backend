using Microsoft.EntityFrameworkCore;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Implementations;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary;
using BaseLibrary.Models;
using Server.Services;
using Microsoft.AspNetCore.Identity;
using BaseLibrary.DTOs;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000",
                                              "https://localhost:7262"
                                              )
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                      });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});


// Add services to the container.
// // register db context
builder.Services.AddDbContext<Glo2GoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection")
    ?? throw new InvalidOperationException("Sorry the connection is not found")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}, The server is starting...");

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
builder.Services.AddScoped<ISiteAccount, SiteRepository>();
builder.Services.AddScoped<ISiteReview, ReviewRepository>();
builder.Services.AddScoped<ITimetable, TimetableRepository>();
builder.Services.AddScoped<IActivity, ActivityRepository>();
builder.Services.AddScoped<IReport, ReportRepository>();
builder.Services.AddScoped<IDashboard, DashboardRepository>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddHostedService<OtpCleanupService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();

