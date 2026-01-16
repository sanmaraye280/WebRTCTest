//using WebRTCTest.Controllers.Hubs;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


//// Add SignalR
//builder.Services.AddSignalR();

//var app = builder.Build();

//app.UseHttpsRedirection();
//app.UseDefaultFiles();
//app.UseStaticFiles();


//_ = app.MapOpenApi("/docs/openapi/{documentName}.json");
//app.MapScalarApiReference("/docs", options =>
//{
//    options.DefaultOpenAllTags = false;

//    options.OpenApiRoutePattern = "/docs/openapi/v1.json";

//    options.HideClientButton = true;

//    options.HideModels = true;

//    options.Theme = ScalarTheme.BluePlanet;
//    options.Layout = ScalarLayout.Classic;

//    options.AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme);
//    options.AddHttpAuthentication(JwtBearerDefaults.AuthenticationScheme, auth =>
//    {
//        auth.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
//    });

//    if (app.Environment.IsStaging())
//        _ = options.AddServer(new ScalarServer("https://localhost:7290") { Description = "Developer Mode" });

//    if (app.Environment.IsDevelopment())
//        _ = options.AddServer(new ScalarServer("https://telehealth.esoftmm.com")
//        { Description = "UAT Local Mode" });

//});


//app.UseAuthorization();

//app.MapControllers();

//// Map hub endpoint
//app.MapHub<CallHub>("/callHub");

//app.MapGet("/", () => "WebRTC SignalR API running...");

//app.Run();


using WebRTCTest.Controllers.Hubs;
using Scalar.AspNetCore;

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

