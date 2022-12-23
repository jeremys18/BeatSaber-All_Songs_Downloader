using BeatSaberDownloader.Data.DBContext;
using BeatSaberSongDownloader.Server.ExtentionMethods;
using BeatSaberSongDownloader.Server.Services.SongDownloader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.Services.AddDbContext<BeatSaverContext>();
builder.Services.AddCronJob<SongDownloadService>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Local;
    c.CronExpression = @"33 14 * * *";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
