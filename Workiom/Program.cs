using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Workiom.Common;
using Workiom.Common.DTOs;
using Workiom.Config;
using Workiom.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<CompanyService>();
builder.Services.AddSingleton<ContactService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CORS Policy
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

//Logging
//app.UseMiddleware<LoggingMiddleware>();

// Error Handling
app.UseExceptionHandler(
    options =>
    {
        options.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

            //Here we can send a Telegram message to alert if there's any 500 internal server error

            if (null != exceptionObject)
            {
                var error = new ResponseDTO()
                {
                    Success = false,
                    Data = exceptionObject.Error.Message
                };
                await context.Response.WriteAsJsonAsync(error).ConfigureAwait(false);
            }
        });
    }
);

app.UseAuthorization();

app.MapControllers();

app.Run();
