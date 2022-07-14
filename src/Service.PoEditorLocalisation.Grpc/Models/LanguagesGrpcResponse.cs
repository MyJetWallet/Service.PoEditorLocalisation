using System.Runtime.Serialization;

namespace Service.PoEditorLocalisation.Grpc.Models
{
	[DataContract]
	public class LanguagesGrpcResponse
	{
		[DataMember(Order = 1)]
		public LanguageGrpcModel[] Items { get; set; }
	}
}