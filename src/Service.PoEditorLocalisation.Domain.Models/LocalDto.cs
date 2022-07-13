namespace Service.PoEditorLocalisation.Domain.Models
{
	public class LocalDto
	{
		public LocalDto()
		{
		}

		public LocalDto(string key, string value, string source)
		{
			term = $"{source}.{key}";
			definition = value;
			reference = source;
		}

		public string term { get; set; }
		public string definition { get; set; }
		public string reference { get; set; }
		public string comment { get; set; }

		public string GetTerm() => term.Replace($"{reference}.", string.Empty);
	}
}