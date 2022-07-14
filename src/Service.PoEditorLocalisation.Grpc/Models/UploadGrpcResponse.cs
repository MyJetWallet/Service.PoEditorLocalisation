using System.Runtime.Serialization;

namespace Service.PoEditorLocalisation.Grpc.Models
{
	[DataContract]
	public class UploadGrpcResponse
	{
		[DataMember(Order = 1)]
		public bool Successful { get; set; }

		[DataMember(Order = 2)]
		public string ErrorText { get; set; }

		[DataMember(Order = 3)]
		public int TermsParsed { get; set; }

		[DataMember(Order = 4)]
		public int TermsAdded { get; set; }

		[DataMember(Order = 5)]
		public int TermsDeleted { get; set; }

		[DataMember(Order = 6)]
		public int TranslationsParsed { get; set; }

		[DataMember(Order = 7)]
		public int TranslationsAdded { get; set; }

		[DataMember(Order = 8)]
		public int TranslationsDeleted { get; set; }
	}
}