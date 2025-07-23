using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Aspire.Wcf.Demo.Contracts;

namespace Aspire.Wcf.Demo.ApiService;

public class DingClient
{
	private readonly IConfiguration _config;

	public DingClient(IConfiguration config)
	{
		_config = config;
	}

	public string CallDing(string name)
	{
		var baseUri = _config
			.GetSection("services:wcfservice:https")
			.GetChildren()
			.FirstOrDefault()
			?.Value ?? throw new InvalidOperationException("WCF endpoint not found");

		var endpoint = new EndpointAddress($"{baseUri}/DingService.svc");
		var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

		var factory = new ChannelFactory<IDingService>(binding, endpoint);
		var client = factory.CreateChannel();

		using var scope = new OperationContextScope((IContextChannel)client);
		OperationContext.Current.OutgoingMessageHeaders.Add(
			MessageHeader.CreateHeader("traceparent", "", Activity.Current?.Id));

		return client.Ding(name);
	}
}