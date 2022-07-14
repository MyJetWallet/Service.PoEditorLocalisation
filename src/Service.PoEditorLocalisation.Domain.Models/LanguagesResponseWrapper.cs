using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Domain.Models ;

	public class LanguagesResponseWrapper
	{
		[JsonProperty("response")]
		public ResponseDto Response { get; set; }

		[JsonProperty("result")]
		public LanguagesResultTermsDto Result { get; set; }

		public class LanguagesResultTermsDto
		{
			[JsonProperty("languages")]
			public LanguageResultInfoDto[] Items { get; set; }
		}

		public class LanguageResultInfoDto
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("code")]
			public string Code { get; set; }
		}
	}