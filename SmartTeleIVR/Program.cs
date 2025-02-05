using Microsoft.OpenApi.Models;
using SmartTeleIVR.Core.Application;
using Twilio;
using Twilio.Clients;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        //Title = "IVR API",
        Version = "v1" 
    });
});

//builder.Services.AddSingleton<ITwilioRestClient>(serviceProvider =>
//{
//    var twilioClient = new TwilioRestClient(
//        builder.Configuration["Twilio:AccountSid"],
//        builder.Configuration["Twilio:AuthToken"]
//    );
//    return twilioClient;
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

builder.Services.AddScoped<IIVRService, IVRService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IVR API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();
//app.UseCors("AllowAll");

app.MapControllers();

app.Run();
