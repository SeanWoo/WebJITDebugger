using WebJITDebugger.Filters;
using WebJITDebugger.Services;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddMvcCore(options => 
{
    options.Filters.Add<ExceptionFilter>();
});
builder.Services.AddTransient<DisassemblerService>();
builder.Services.AddTransient<ProcessService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(config => 
        config.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
}*/

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(config =>
    config.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
);

app.Use(async (context, next) =>
{
    var logger = app.Services.GetService<ILogger<Program>>();
    logger.LogInformation($"{context.Request.Path}");
    await next.Invoke();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();