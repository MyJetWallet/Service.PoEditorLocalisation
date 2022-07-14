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

namespace Service.PoEditorLocalisation.Services
{
	public class PoEditorSender : IPoEditorSender
	{
		private readonly ILogger<PoEditorSender> _logger;

		public PoEditorSender(ILogger<PoEditorSender> logger)
		{
			_logger = logger;
		}

		public async ValueTask<UploadResult> Upload(List<LocalDto> dtos, string lang)
		{
			if (!dtos.Any())
				return UploadResult.ErrorResult("No templates to upload");

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

			string jsonData = JsonConvert.SerializeObject(dtos);

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

					return UploadResult.ErrorResult(message);
				}
			}

			string responseContent = response.Content.ReadAsStringAsync().Result;
			if (string.IsNullOrWhiteSpace(responseContent))
			{
				_logger.LogError("Error while upload from PoEditor, no content recieved");

				return UploadResult.ErrorResult("Empty contents recieved from service");
			}

			var responseData = JsonConvert.DeserializeObject<UploadResponseWrapper>(responseContent);
			if (responseData == null)
			{
				_logger.LogError("Error while upload from PoEditor, content: {content}", responseContent);

				return UploadResult.ErrorResult("Invalid json contents recieved from service");
			}

			if (responseData.Response.IsFail())
				return UploadResult.ErrorResult(responseData.Response.Message);

			return new UploadResult
			{
				Successful = true,
				Results = responseData.Result
			};
		}

		public async Task<DownloadResult> Download(string lang)
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

			string downloadUrl = Program.Settings.PoEditorDownloadUrl;

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

				return DownloadResult.ErrorResult(message);
			}

			string responseContent = response.Content.ReadAsStringAsync().Result;
			if (string.IsNullOrWhiteSpace(responseContent))
			{
				_logger.LogError("Error while upload from PoEditor, no content recieved");

				return DownloadResult.ErrorResult("Empty contents recieved from service");
			}

			_logger.LogDebug("Recieved: {@data}", responseContent);

			var responseData = JsonConvert.DeserializeObject<DownloadResponseWrapper>(responseContent);
			if (responseData == null)
			{
				_logger.LogError("Error while download from PoEditor, content: {content}", responseContent);

				return DownloadResult.ErrorResult("Invalid json contents recieved from service");
			}

			if (responseData.Response.IsFail())
			{
				_logger.LogError("Error recieved from PoEditor, reponse: {@response}", responseData);

				return DownloadResult.ErrorResult(responseData.Response.Message);
			}

			LocalDto[] items = responseData.Result.Terms.Select(dto => new LocalDto
			{
				Term = dto.Term,
				Comment = dto.Comment,
				Reference = dto.Reference,
				Definition = dto.Translation.Content
			}).ToArray();

			return new DownloadResult
			{
				Successful = true,
				Results = items
			};
		}
	}
}