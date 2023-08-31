using Microsoft.AspNetCore.Mvc.Formatters;
using sodoff.Model;
using sodoff.Services;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings() { OmitXmlDeclaration = false }));
    options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
});
builder.Services.AddDbContext<DBContext>();
builder.Services.AddScoped<KeyValueService>();
builder.Services.AddSingleton<ItemService>();
builder.Services.AddSingleton<MissionStoreSingleton>();
builder.Services.AddScoped<MissionService>();
builder.Services.AddSingleton<StoreService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddSingleton<AchievementService>();
builder.Services.AddScoped<InventoryService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<DBContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
