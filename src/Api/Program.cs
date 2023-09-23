using Api;
using Duende.Bff.Yarp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")).AddBffExtensions();
}

builder.ConfigureBff();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure BFF
app.UseAuthentication();
app.UseRouting();
app.UseBff();
app.UseAuthorization();

// Configure BFF endpoints
app.MapBffManagementEndpoints();

app.MapControllers().RequireAuthorization().AsBffApiEndpoint();

if (app.Environment.IsDevelopment())
{
    // Serve frontend through proxy in development
    // Needs to be at the end to catch controllers etc..
    app.MapReverseProxy();
}
else
{
    // Serve frontend as static files in production
    app.MapFallbackToFile("/index.html");
}

app.Run();
