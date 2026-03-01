using System.Text.Json.Serialization;
using RealtimeDashboard.API.Middleware;
using RealtimeDashboard.API.SeedData;
using RealtimeDashboard.Application;
using RealtimeDashboard.Infrastructure;
using RealtimeDashboard.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Realtime Operations Dashboard API",
        Version = "v1",
        Description = """
                      REST API for real-time resource availability monitoring.

                      **Demo domain:** Hospital resource management (beds, rooms, equipment).
                      The same architecture applies to logistics, trading floors, and SaaS operations.

                      **Key flows:**
                      - `POST /api/resources` — create a resource
                      - `PATCH /api/resources/{id}/status` — update status (triggers alerts if threshold matched)
                      - `GET /api/alerts/active` — poll active alerts (replaced by SignalR in the dashboard)
                      """
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await ApplicationDbContextSeed.SeedAsync(context);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Realtime Dashboard API v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();