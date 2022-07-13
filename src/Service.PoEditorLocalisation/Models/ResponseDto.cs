using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Models
{
	public class ResponseDto
	{
		public const string FailStatus = "fail";

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }
	}
}