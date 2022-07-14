using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Domain.Models
{
	public class ResponseDto
	{
		public const string FailStatus = "fail";
		public const string SuccessStatus = "success";

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		public bool IsFail() => Status is FailStatus or not SuccessStatus;
	}
}