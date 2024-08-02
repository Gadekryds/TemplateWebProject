using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TemplateWebProject.Dependency;

public static class MonitoringDependencyInjection
{
    public static WebApplicationBuilder AddMonitoring(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        Uri otelAgent = new(builder.Configuration.GetValue<string>("OTEL_AGENT")!);
        builder.Logging.AddOpenTelemetry();

        builder.Services.AddOpenTelemetry()
                        .ConfigureResource(resource =>
                            resource.AddService(DiagnosticsConfig.ServiceName)
                                    .AddAttributes([new("app_name", DiagnosticsConfig.ServiceName)]))
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

        return builder;
    }
}
