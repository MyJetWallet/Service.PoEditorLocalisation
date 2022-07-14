using System.Collections.Generic;
using System.Threading.Tasks;
using Service.PoEditorLocalisation.Domain.Models;

namespace Service.PoEditorLocalisation.Domain
{
	public interface IPoEditorSender
	{
		ValueTask<UploadResult> Upload(List<LocalDto> dtos, string lang);

		Task<DownloadResult> Download(string lang);
		
		Task<(string name, string code)[]> GetLanguages();
	}
}