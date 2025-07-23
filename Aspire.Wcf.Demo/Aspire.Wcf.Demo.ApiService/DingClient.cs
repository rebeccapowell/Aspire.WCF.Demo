using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Aspire.Wcf.Demo.Contracts;
using OpenTelemetry;

namespace Aspire.Wcf.Demo.ApiService;

public class DingClient(IConfiguration config)
{
	public string CallDing(string name)
	{
		var baseUri = config
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
		Baggage.SetBaggage("caller", "Aspire.ApiService");
		return client.Ding(name);
	}
}