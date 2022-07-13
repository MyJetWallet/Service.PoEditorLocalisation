using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Models
{
	public class UploadResponseWrapper
	{
		[JsonProperty("response")]
		public ResponseDto Response { get; set; }

		[JsonProperty("result")]
		public UploadResultTermsDto Result { get; set; }

		public class UploadResultTermsDto
		{
			[JsonProperty("terms")]
			public UploadResultTermInfoDto[] Terms { get; set; }
		}

		public class UploadResultTermInfoDto
		{
			[JsonProperty("term")]
			public string Term { get; set; }

			[JsonProperty("reference")]
			public string Reference { get; set; }

			[JsonProperty("comment")]
			public string Comment { get; set; }

			[JsonProperty("translation")]
			public UploadResultTermTranslationInfoDto Translation { get; set; }
		}

		public class UploadResultTermTranslationInfoDto
		{
			[JsonProperty("content")]
			public string Content { get; set; }
		}
	}
}