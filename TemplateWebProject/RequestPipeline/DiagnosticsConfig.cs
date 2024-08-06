using System.Diagnostics.Metrics;

namespace TemplateWebProject;

public static class DiagnosticsConfig
{
    public const string ServiceName = "template-webapi";
    public static Meter Meter = new Meter(ServiceName);

    public static Counter<int> WeatherforeCastCounter = Meter.CreateCounter<int>("weather_forecast_counter");

}
