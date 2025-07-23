using System.ServiceModel;
using CoreWCF;

namespace Aspire.Wcf.Demo.Contracts;

[System.ServiceModel.ServiceContract]
[CoreWCF.ServiceContract] // optional if only used server-side
public interface IDingService
{
	[System.ServiceModel.OperationContract]
	[CoreWCF.OperationContract] // optional
	string Ding(string name);
}