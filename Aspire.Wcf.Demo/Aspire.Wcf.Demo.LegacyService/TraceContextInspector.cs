using System.Diagnostics;
using CoreWCF.Dispatcher;

namespace Aspire.Wcf.Demo.LegacyService;

public class TraceContextInspector : IDispatchMessageInspector
{
	public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
	{
		var headers = OperationContext.Current.IncomingMessageHeaders;
		var traceparentIndex = headers.FindHeader("traceparent", "");

		if (traceparentIndex != -1)
		{
			var traceparent = headers.GetHeader<string>(traceparentIndex);
			Activity.Current?.SetTag("client.traceparent", traceparent);
		}

		return null;
	}

	public void BeforeSendReply(ref Message reply, object correlationState) { }
}