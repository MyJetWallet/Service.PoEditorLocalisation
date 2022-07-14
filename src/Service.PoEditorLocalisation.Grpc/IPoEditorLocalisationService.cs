using System.ServiceModel;
using System.Threading.Tasks;
using Service.PoEditorLocalisation.Grpc.Models;

namespace Service.PoEditorLocalisation.Grpc
{
	[ServiceContract]
	public interface IPoEditorLocalisationService
	{
		[OperationContract]
		Task<UploadGrpcResponse> UploadAsync(ExportGrpcRequest request);

		[OperationContract]
		Task<DownloadGrpcResponse> DownloadAsync(ExportGrpcRequest request);
	}
}