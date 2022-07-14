using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Domain.Models
{
	public class DownloadResponseWrapper
	{
		[JsonProperty("response")]
		public ResponseDto Response { get; set; }

		[JsonProperty("result")]
		public DownloadResultTermsDto Result { get; set; }

		public class DownloadResultTermsDto
		{
			[JsonProperty("terms")]
			public DownloadResultTermInfoDto[] Terms { get; set; }
		}

		public class DownloadResultTermInfoDto
		{
			[JsonProperty("term")]
			public string Term { get; set; }

			[JsonProperty("reference")]
			public string Reference { get; set; }

			[JsonProperty("comment")]
			public string Comment { get; set; }

			[JsonProperty("translation")]
			public DownloadResultTermTranslationInfoDto Translation { get; set; }
		}

		public class DownloadResultTermTranslationInfoDto
		{
			[JsonProperty("content")]
			public string Content { get; set; }
		}
	}
}