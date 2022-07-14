namespace Service.PoEditorLocalisation.Domain.Models ;

	public class UploadResult
	{
		public bool Successful { get; set; }

		public string ErrorText { get; set; }

		public UploadResponseWrapper.UploadResultDto Results { get; set; }

		public static UploadResult ErrorResult(string text) => new()
		{
			Successful = false,
			ErrorText = text
		};
	}