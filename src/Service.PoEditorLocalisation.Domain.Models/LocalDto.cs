using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Domain.Models
{
	public class LocalDto
	{
		public LocalDto()
		{
		}

		public LocalDto(string key, string value, string source)
		{
			Term = $"{source}.{key}";
			Definition = value;
			Reference = source;
		}

		[JsonProperty("term")]
		public string Term { get; set; }

		[JsonProperty("definition")]
		public string Definition { get; set; }

		[JsonProperty("reference")]
		public string Reference { get; set; }

		[JsonProperty("comment")]
		public string Comment { get; set; }

		public string GetTerm() => Term.Replace($"{Reference}.", string.Empty);
	}
}