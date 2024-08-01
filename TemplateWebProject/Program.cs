using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TemplateWebProject;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();


Uri otelAgent = new(builder.Configuration.GetValue<string>("OTEL_AGENT")!);
string serviceName = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME")!;


builder.Logging.AddOpenTelemetry();

builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => 
                    resource.AddService(serviceName)
                            .AddAttributes([new("app_name", serviceName)]))
                .WithLogging(logs =>
                {
                    logs.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = otelAgent;
                    });
                })
                
                .WithMetrics(metrics =>
                {
                    metrics.AddMeter(
                        "Microsoft.AspNetCore.Hosting", 
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        DiagnosticsConfig.Meter.Name);
                    
                    metrics.AddHttpClientInstrumentation();
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = otelAgent;
                    });
                })
                .WithTracing(traces =>
                {
                    traces.AddHttpClientInstrumentation();
                    traces.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = otelAgent;
                    });

                });

builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
