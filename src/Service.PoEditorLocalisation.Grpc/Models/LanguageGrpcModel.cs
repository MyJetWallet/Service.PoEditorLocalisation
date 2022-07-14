using System.Runtime.Serialization;

namespace Service.PoEditorLocalisation.Grpc.Models
{
	[DataContract]
	public class LanguageGrpcModel
	{
		[DataMember(Order = 1)]
		public string Name { get; set; }

		[DataMember(Order = 2)]
		public string Code { get; set; }
	}
}