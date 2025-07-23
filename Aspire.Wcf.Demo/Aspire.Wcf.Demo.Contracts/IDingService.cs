using System.ServiceModel;

namespace Aspire.Wcf.Demo.Contracts;

/// <summary>
///     This is annoying that I need both here to share the contract between client and server
/// </summary>
[ServiceContract]
[CoreWCF.ServiceContract] // optional if only used server-side
public interface IDingService
{
	[OperationContract]
	[CoreWCF.OperationContract] // optional
	string Ding(string name);
}