using Microsoft.AspNetCore.Mvc.Formatters;
using sodoff.Model;
using sodoff.Services;
using sodoff.Utils;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings() { OmitXmlDeclaration = false }));
    options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    options.Filters.Add<LogRequestOnError>();
});
builder.Services.AddDbContext<DBContext>();

builder.Services.AddSingleton<MissionStoreSingleton>();
builder.Services.AddSingleton<AchievementStoreSingleton>();
builder.Services.AddSingleton<ItemService>();
builder.Services.AddSingleton<StoreService>();

builder.Services.AddScoped<KeyValueService>();
builder.Services.AddScoped<MissionService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<AchievementService>();
builder.Services.AddScoped<GameDataService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<DBContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
