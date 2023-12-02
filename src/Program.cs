using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using sodoff.Configuration;
using sodoff.Middleware;
using sodoff.Model;
using sodoff.Services;
using sodoff.Utils;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AssetServerConfig>(builder.Configuration.GetSection("AssetServer"));
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

bool assetServer = builder.Configuration.GetSection("AssetServer").GetValue<bool>("Enabled");
int assetPort = builder.Configuration.GetSection("AssetServer").GetValue<int>("Port");
if (assetServer)
    builder.Services.Configure<KestrelServerOptions>(options => {
        options.ListenAnyIP(assetPort);
    });

var app = builder.Build();

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<DBContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.

if (assetServer)
    app.UseMiddleware<AssetMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
