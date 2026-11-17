using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Deposita.Api.Infrastructure;
 using Deposita.Application;
 using Deposita.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Deposita Cash Operations API",
        Version = "v1"
    });
});


builder.Services.AddApplication();
 builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
    options.AddPolicy("AngularClient", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Deposita Cash Operations API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AngularClient");
app.UseAuthorization();
app.MapControllers();

app.Run();