using ThinkEdu_Question_Service.Application;
using FastEndpoints;
using Scalar.AspNetCore;
using ThinkEdu_Question_Service.Infrastructure;
using ThinkEdu_Question_Service.Domain.Extensions;
using Serilog;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using FastEndpoints.Swagger;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddFastEndpoints()
    .SwaggerDocument(opt =>
    {
        opt.DocumentSettings = settings =>
        {
            settings.Title = "ThinkEdu Question Service API";
            settings.Version = "v1";
        };
    });

builder.AddAppConfigurations();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new("vi-VN"),
            new("en-US"),
        };

        options.DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwaggerGen();
}

app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

app.UseRequestLocalization();

app.AddApplicationBuilders();

app.UseHttpsRedirection();

app.MapControllers();

app.MigrateDatabase();

app.Run();