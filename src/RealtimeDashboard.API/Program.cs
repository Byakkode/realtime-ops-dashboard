using RealtimeDashboard.API.Hubs;
using RealtimeDashboard.API.Middleware;
using RealtimeDashboard.API.SeedData;
using RealtimeDashboard.Application;
using RealtimeDashboard.Application.Common.Interfaces;
using RealtimeDashboard.Infrastructure;
using RealtimeDashboard.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddScoped<IRealtimeNotifier, ResourceHubNotifier>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(origin =>
                    new Uri(origin).Host == "localhost")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else
        {
            policy.WithOrigins("https://OnVerraMonNomDeDomaine.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await ApplicationDbContextSeed.SeedAsync(context);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("BlazorClient");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Realtime Dashboard API v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();
app.MapHub<ResourceHub>("/hubs/resources");

app.Run();