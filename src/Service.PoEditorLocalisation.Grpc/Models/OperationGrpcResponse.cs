using System.Runtime.Serialization;

namespace Service.PoEditorLocalisation.Grpc.Models
{
	[DataContract]
	public class OperationGrpcResponse
	{
		[DataMember(Order = 1)]
		public bool Successful { get; set; }

		[DataMember(Order = 2)]
		public string ErrorText { get; set; }

		[DataMember(Order = 3)]
		public int TemplatesChanged { get; set; }

		[DataMember(Order = 4)]
		public int SmsTemplatesChanged { get; set; }

		[DataMember(Order = 5)]
		public int PushTemplatesChanged { get; set; }
	}
}