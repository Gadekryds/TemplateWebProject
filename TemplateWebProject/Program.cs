using TemplateWebProject.Dependency;
using TemplateWebProject.RequestPipeline;

var builder = WebApplication.CreateBuilder(args);

builder.AddMonitoring();
builder.AddExceptionHandling();

builder.Services.AddControllers();

var app = builder.Build();

app.UseGlobalErrorHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/throw", () =>
{
    throw new Exception("test");
});

app.Run();
