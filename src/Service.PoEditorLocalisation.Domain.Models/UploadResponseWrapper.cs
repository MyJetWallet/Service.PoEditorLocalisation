using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Domain.Models
{
	public class UploadResponseWrapper
	{
		[JsonProperty("response")]
		public ResponseDto Response { get; set; }

		[JsonProperty("result")]
		public UploadResultDto Result { get; set; }

		public class UploadResultDto
		{
			[JsonProperty("terms")]
			public UploadResultItemDto Terms { get; set; }

			[JsonProperty("translations")]
			public UploadResultItemDto Translations { get; set; }
		}

		public class UploadResultItemDto
		{
			[JsonProperty("parsed")]
			public int Parsed { get; set; }

			[JsonProperty("added")]
			public int Added { get; set; }

			[JsonProperty("deleted")]
			public int Deleted { get; set; }
		}
	}
}