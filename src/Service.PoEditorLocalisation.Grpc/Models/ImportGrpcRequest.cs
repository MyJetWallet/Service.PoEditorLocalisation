using System.Runtime.Serialization;

namespace Service.PoEditorLocalisation.Grpc.Models
{
	[DataContract]
	public class ImportGrpcRequest
	{
		[DataMember(Order = 1)]
		public string Lang { get; set; }
	}
}