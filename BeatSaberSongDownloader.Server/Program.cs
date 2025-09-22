using BeatSaberDownloader.Data.DBContext;
using BeatSaberSongDownloader.Server.ExtentionMethods;
using BeatSaberSongDownloader.Server.Services.SongDownloader;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication();
builder.Services.AddDbContext<BeatSaverContext>();
//builder.Services.AddCronJob<SongDownloadService>(c =>
//{
//    c.TimeZoneInfo = TimeZoneInfo.Local;
//    c.CronExpression = @"00 08 * * *";
//});

builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:5000");

//app.UseAuthentication();

app.MapControllers();

app.Run();
