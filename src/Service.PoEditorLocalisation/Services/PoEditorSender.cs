using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.PoEditorLocalisation.Domain;
using Service.PoEditorLocalisation.Domain.Models;
using Service.PoEditorLocalisation.Models;

namespace Service.PoEditorLocalisation.Services
{
	public class PoEditorSender : IPoEditorSender
	{
		private readonly ILogger<PoEditorSender> _logger;

		public PoEditorSender(ILogger<PoEditorSender> logger)
		{
			_logger = logger;
		}

		public async Task<(bool successful, string error)> Upload(string jsonData, string lang)
		{
			var multiForm = new MultipartFormDataContent();

			var parameters = new Dictionary<string, string>
			{
				{"api_token", Program.Settings.PoEditorApiToken},
				{"id", Program.Settings.PoEditorBackendProjectId},
				{"updating", "terms_translations"},
				{"language", lang},
				{"overwrite", "1"},
				{"sync_terms", "0"},
				{"tags", "obsolete"},
				{"fuzzy_trigger", "1"}
			};

			foreach (KeyValuePair<string, string> keyValuePair in parameters)
				multiForm.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);

			HttpResponseMessage response;

			using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonData)))
			{
				content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
				multiForm.Add(content, "file", "contents.json");

				string url = Program.Settings.PoEditorUploadUrl;

				_logger.LogInformation("Request upload to PoEditor, url: {url}, params: {@params}", url, parameters);

				try
				{
					response = await new HttpClient().PostAsync(url, multiForm);
				}
				catch (Exception ex)
				{
					string message = ex.Message;

					_logger.LogError("Error while upload items from PoEditor, message: {message}, lang: {lang}", message, lang);

					return (false, message);
				}
			}

			string responseContent = response.Content.ReadAsStringAsync().Result;

			var responseData = JsonConvert.DeserializeObject<UploadResponseWrapper>(responseContent);

			bool isError = responseData?.Response?.Status == ResponseDto.FailStatus;
			string error = isError
				? responseData.Response.Message
				: null;

			return (!isError, error);
		}

		public async Task<(LocalDto[] items, string error)> Download(string lang)
		{
			var multiForm = new MultipartFormDataContent();

			var parameters = new Dictionary<string, string>
			{
				{"api_token", Program.Settings.PoEditorApiToken},
				{"id", Program.Settings.PoEditorBackendProjectId},
				{"language", lang}
			};

			foreach (KeyValuePair<string, string> keyValuePair in parameters)
				multiForm.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);

			string downloadUrl = Program.Settings.PoEditorExportUrl;

			_logger.LogInformation("Request download to PoEditor, url: {url}, params: {@params}", downloadUrl, parameters);

			HttpResponseMessage response;

			try
			{
				response = await new HttpClient().PostAsync(downloadUrl, multiForm);
			}
			catch (Exception ex)
			{
				string message = ex.Message;

				_logger.LogError(message);

				return (null, message);
			}

			string responseContent = response.Content.ReadAsStringAsync().Result;

			var responseData = JsonConvert.DeserializeObject<UploadResponseWrapper>(responseContent);
			if (responseData == null)
			{
				_logger.LogError("Error while download from PoEditor, content: {content}", responseContent);

				return (null, "Invalid json contents recieved from service");
			}

			if (responseData.Response?.Status == ResponseDto.FailStatus)
			{
				_logger.LogError("Error recieved from PoEditor, reponse: {@response}", responseData);

				return (null, responseData.Response.Message);
			}

			LocalDto[] items = responseData.Result.Terms.Select(dto => new LocalDto
			{
				term = dto.Term,
				comment = dto.Comment,
				reference = dto.Reference,
				definition = dto.Translation.Content
			}).ToArray();

			return (items, null);
		}
	}
}