using System.Diagnostics;
using Aspire.Wcf.Demo.Contracts;

namespace Aspire.Wcf.Demo.LegacyService;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class DingService : IDingService
{
	private readonly ILogger<DingService> _logger;
	static readonly ActivitySource Src = new("CoreWCF.Ding");

	public DingService(ILogger<DingService> logger)
	{
		_logger = logger;
	}
	public string Ding(string name)
	{
		using var act = Src.StartActivity("DingService.Ding", ActivityKind.Server);
		
		_logger.LogInformation("Request in legacy DingService {Name}", name);
		return $"Pong: {name}";
	}
}