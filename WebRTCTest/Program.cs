using Scalar.AspNetCore;
using WebRTCTest.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

_ = app.MapOpenApi("/docs/openapi/{documentName}.json");

// S C A L A R  (without JwtBearer)
app.MapScalarApiReference("/docs", options =>
{
    options.DefaultOpenAllTags = false;
    options.OpenApiRoutePattern = "/docs/openapi/v1.json";
    options.HideClientButton = true;
    options.HideModels = true;
    options.Theme = ScalarTheme.BluePlanet;
    options.Layout = ScalarLayout.Classic;

    // ❌ Removed JWT scheme — using no authentication.
    // If you want, you can define custom text token:
    options.AddHttpAuthentication("ApiKey", auth =>
    {
        auth.Token = "test-token-123";
        //auth.HeaderName = "X-API-KEY";
    });

    if (app.Environment.IsStaging())
        _ = options.AddServer(new ScalarServer("https://localhost:7290")
        { Description = "Developer Mode" });

    if (app.Environment.IsDevelopment())
        _ = options.AddServer(new ScalarServer("https://localhost:7290")
        { Description = "UAT Local Mode" });
});

app.UseAuthorization();

app.MapControllers();

app.MapHub<CallHub>("/callHub");

app.MapGet("/", () => "WebRTC SignalR API running...");

app.Run();

