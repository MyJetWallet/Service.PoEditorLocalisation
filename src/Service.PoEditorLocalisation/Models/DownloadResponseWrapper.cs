using Newtonsoft.Json;

namespace Service.PoEditorLocalisation.Models
{
	public class DownloadResponseWrapper
	{
		[JsonProperty("response")]
		public ResponseDto Response { get; set; }
	}
}