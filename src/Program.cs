using Microsoft.AspNetCore.Mvc.Formatters;
using sodoff.Model;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings() { OmitXmlDeclaration = false }));
});
builder.Services.AddDbContext<DBContext>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<DBContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
