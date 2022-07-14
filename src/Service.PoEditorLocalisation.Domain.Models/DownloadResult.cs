namespace Service.PoEditorLocalisation.Domain.Models ;

	public class DownloadResult
	{
		public bool Successful { get; set; }

		public string ErrorText { get; set; }

		public LocalDto[] Results { get; set; }

		public static DownloadResult ErrorResult(string text) => new()
		{
			Successful = false,
			ErrorText = text
		};
	}