using System.Threading.Tasks;
using Service.PoEditorLocalisation.Domain.Models;

namespace Service.PoEditorLocalisation.Domain
{
	public interface IPoEditorSender
	{
		Task<(bool successful, string error)> Upload(string jsonData, string lang);

		Task<(LocalDto[] items, string error)> Download(string lang);
	}
}