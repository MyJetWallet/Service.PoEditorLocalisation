using System.ServiceModel;
using System.Threading.Tasks;
using Service.PoEditorLocalisation.Grpc.Models;

namespace Service.PoEditorLocalisation.Grpc
{
	[ServiceContract]
	public interface ILocalisationService
	{
		[OperationContract]
		Task<OperationGrpcResponse> ExportAsync(ExportGrpcRequest request);

		[OperationContract]
		Task<OperationGrpcResponse> ImportAsync(ExportGrpcRequest request);
	}
}