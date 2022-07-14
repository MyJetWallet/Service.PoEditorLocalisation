using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.PoEditorLocalisation.Settings
{
	public class SettingsModel
	{
		[YamlProperty("PoEditorLocalisation.SeqServiceUrl")]
		public string SeqServiceUrl { get; set; }

		[YamlProperty("PoEditorLocalisation.ZipkinUrl")]
		public string ZipkinUrl { get; set; }

		[YamlProperty("PoEditorLocalisation.ElkLogs")]
		public LogElkSettings ElkLogs { get; set; }

		[YamlProperty("PoEditorLocalisation.MyNoSqlWriterUrl")]
		public string MyNoSqlWriterUrl { get; set; }

		[YamlProperty("PoEditorLocalisation.MyNoSqlReaderHostPort")]
		public string MyNoSqlReaderHostPort { get; set; }

		[YamlProperty("PoEditorLocalisation.PoEditorApiToken")]
		public string PoEditorApiToken { get; set; }

		[YamlProperty("PoEditorLocalisation.PoEditorUploadUrl")]
		public string PoEditorUploadUrl { get; set; }

		[YamlProperty("PoEditorLocalisation.PoEditorDownloadUrl")]
		public string PoEditorDownloadUrl { get; set; }

		[YamlProperty("PoEditorLocalisation.PoEditorLanguagesUrl")]
		public string PoEditorLanguagesUrl { get; set; }

		[YamlProperty("PoEditorLocalisation.PoEditorBackendProjectId")]
		public string PoEditorBackendProjectId { get; set; }
	}
}