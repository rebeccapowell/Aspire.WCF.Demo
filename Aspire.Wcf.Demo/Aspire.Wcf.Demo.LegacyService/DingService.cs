using System.Diagnostics;
using System.Diagnostics.Metrics;
using Aspire.Wcf.Demo.Contracts;
using OpenTelemetry;

namespace Aspire.Wcf.Demo.LegacyService;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class DingService(ILogger<DingService> logger) : IDingService
{
	private static readonly ActivitySource ActivitySource = new("CoreWCF.Ding");
	private static readonly Meter Meter = new("CoreWCF.Ding");
	private static readonly Counter<long> DingCounter = Meter.CreateCounter<long>("ding.calls");
	private static readonly Histogram<double> DingLatency = Meter.CreateHistogram<double>("ding.latency.ms");

	public string Ding(string name)
	{
		var start = Stopwatch.GetTimestamp();

		using var activity = ActivitySource.StartActivity("DingService.Ding", ActivityKind.Server);

		// Optionally extract baggage (if passed down)
		var caller = Baggage.GetBaggage("caller") ?? "unknown";
		activity?.SetTag("caller", caller); // Tag for trace
		logger.LogInformation("Ding called by {Caller} for {Name}", caller, name);

		// Count the call
		DingCounter.Add(1, new KeyValuePair<string, object?>("caller", caller));

		var response = $"Dong: {name}";

		// Record latency
		var durationMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
		DingLatency.Record(durationMs,
			new KeyValuePair<string, object?>("caller", caller),
			new KeyValuePair<string, object?>("env", "dev"));

		logger.LogInformation("Dong as reply for call by {Caller} for {Name}", caller, name);

		return response;
	}

	private static double GetElapsedMilliseconds(long start, long stop)
	{
		return (stop - start) * 1000.0 / Stopwatch.Frequency;
	}
}