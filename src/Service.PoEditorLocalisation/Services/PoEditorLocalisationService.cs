using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.MessageTemplates.Domain.Models.NoSql;
using Service.PoEditorLocalisation.Domain;
using Service.PoEditorLocalisation.Domain.Models;
using Service.PoEditorLocalisation.Grpc;
using Service.PoEditorLocalisation.Grpc.Models;
using Service.PoEditorLocalisation.NoSqlEntities;
using Service.SmsSender.Domain.Models;

namespace Service.PoEditorLocalisation.Services
{
	public class PoEditorLocalisationService : IPoEditorLocalisationService
	{
		private const string MessageTemplateSource = "message-template";
		private const string SmsTemplateSource = "sms";
		private const string PushTemplateSource = "push";

		private readonly ILogger<PoEditorLocalisationService> _logger;
		private readonly IPoEditorSender _poEditorSender;
		private readonly IMyNoSqlServerDataWriter<TemplateNoSqlEntity> _templateWriter;
		private readonly IMyNoSqlServerDataWriter<SmsTemplateMyNoSqlEntity> _smsTemplateWriter;
		private readonly IMyNoSqlServerDataWriter<PushTemplateNoSqlEntity> _pushTemplateWriter;

		public PoEditorLocalisationService(ILogger<PoEditorLocalisationService> logger,
			IMyNoSqlServerDataWriter<TemplateNoSqlEntity> templateWriter,
			IMyNoSqlServerDataWriter<SmsTemplateMyNoSqlEntity> smsTemplateWriter,
			IMyNoSqlServerDataWriter<PushTemplateNoSqlEntity> pushTemplateWriter,
			IPoEditorSender poEditorSender)
		{
			_logger = logger;
			_templateWriter = templateWriter;
			_smsTemplateWriter = smsTemplateWriter;
			_pushTemplateWriter = pushTemplateWriter;
			_poEditorSender = poEditorSender;
		}

		public async Task<UploadGrpcResponse> UploadAsync(ExportGrpcRequest request)
		{
			string lang = request.Lang;

			var data = new List<LocalDto>();

			List<TemplateNoSqlEntity> messages = await _templateWriter.GetAsync();
			foreach (TemplateNoSqlEntity msg in messages)
			{
				if (msg.BodiesSerializable.TryGetValue($"{msg.DefaultBrand};-;{lang.ToLower()}", out string body) && !body.StartsWith("Placeholder for"))
					data.Add(new LocalDto(msg.TemplateId, body, MessageTemplateSource));
			}

			List<SmsTemplateMyNoSqlEntity> sms = await _smsTemplateWriter.GetAsync();
			foreach (SmsTemplateMyNoSqlEntity msg in sms)
			{
				BrandLangBody brand = msg.Template.BrandLangBodies.FirstOrDefault(e => e.Brand == msg.Template.DefaultBrand);
				if (brand == null)
					continue;

				if (brand.LangBodies.TryGetValue(lang, out string body))
					data.Add(new LocalDto(msg.RowKey, body, SmsTemplateSource));
			}

			List<PushTemplateNoSqlEntity> push = await _pushTemplateWriter.GetAsync();
			foreach (PushTemplateNoSqlEntity msg in push)
			{
				if (msg.BodiesSerializable.TryGetValue($"{msg.DefaultBrand};-;{lang.ToLower()}", out string body))
					data.Add(new LocalDto(msg.RowKey, body, PushTemplateSource));
			}

			UploadResult result = await _poEditorSender.Upload(data, lang.ToLower());

			var response = new UploadGrpcResponse
			{
				Successful = result.Successful,
				ErrorText = result.ErrorText
			};

			UploadResponseWrapper.UploadResultDto results = result.Results;
			if (results != null)
			{
				response.TermsAdded = results.Terms.Added;
				response.TermsParsed = results.Terms.Parsed;
				response.TermsDeleted = results.Terms.Deleted;
				response.TranslationsAdded = results.Translations.Added;
				response.TranslationsParsed = results.Translations.Parsed;
				response.TranslationsDeleted = results.Translations.Deleted;
			}

			return response;
		}

		public async Task<DownloadGrpcResponse> DownloadAsync(ExportGrpcRequest request)
		{
			string lang = request.Lang;

			DownloadResult result = await _poEditorSender.Download(lang.ToLower());

			var response = new DownloadGrpcResponse
			{
				Successful = result.Successful,
				ErrorText = result.ErrorText
			};

			LocalDto[] results = result.Results;
			if (results == null)
				return response;

			_logger.LogInformation("Recieved {count} items info from PoEditor", results.Length);

			List<TemplateNoSqlEntity> templateNoSqlEntities = await _templateWriter.GetAsync();
			List<SmsTemplateMyNoSqlEntity> smsNoSqlEntities = await _smsTemplateWriter.GetAsync();
			List<PushTemplateNoSqlEntity> pushNoSqlEntities = await _pushTemplateWriter.GetAsync();

			var templatesChanged = 0;
			var smsTemplatesChanged = 0;
			var pushTemplatesChanged = 0;

			foreach (LocalDto item in results)
			{
				string term = item.GetTerm();

				if (item.Reference == MessageTemplateSource)
				{
					TemplateNoSqlEntity entity = templateNoSqlEntities.FirstOrDefault(e => e.TemplateId == term);
					if (entity != null)
					{
						var key = $"{entity.DefaultBrand};-;{lang.ToLower()}";
						if (entity.BodiesSerializable.ContainsKey(key) && entity.BodiesSerializable[key] != item.Definition)
						{
							entity.BodiesSerializable[key] = item.Definition;
							templatesChanged++;
						}
					}
				}

				else if (item.Reference == SmsTemplateSource)
				{
					SmsTemplateMyNoSqlEntity entity = smsNoSqlEntities
						.FirstOrDefault(e => e.RowKey == term && e.Template.BrandLangBodies
							.Any(b => b.Brand == e.Template.DefaultBrand && b.LangBodies.Any(lb => lb.Key == lang)));

					if (entity != null)
					{
						BrandLangBody langBody = entity.Template.BrandLangBodies.First(b => b.Brand == entity.Template.DefaultBrand);
						if (langBody.LangBodies[lang] != item.Definition)
						{
							langBody.LangBodies[lang] = item.Definition;
							smsTemplatesChanged++;
						}
					}
				}

				else if (item.Reference == PushTemplateSource)
				{
					PushTemplateNoSqlEntity entity = pushNoSqlEntities.FirstOrDefault(e => e.RowKey == term);
					if (entity != null)
					{
						var key = $"{entity.DefaultBrand};-;{lang.ToLower()}";
						if (entity.BodiesSerializable.ContainsKey(key) && entity.BodiesSerializable[key] != item.Definition)
						{
							entity.BodiesSerializable[key] = item.Definition;
							pushTemplatesChanged++;
						}
					}
				}
			}

			if (templatesChanged == 0 && smsTemplatesChanged == 0 && pushTemplatesChanged == 0)
				_logger.LogInformation("No templates changed. Nothing to update.");

			if (templatesChanged > 0)
			{
				await _templateWriter.CleanAndBulkInsertAsync(templateNoSqlEntities, DataSynchronizationPeriod.Min1);
				_logger.LogInformation("{cnt} {sqlName} entities updated.", templatesChanged, nameof(TemplateNoSqlEntity));
			}

			if (smsTemplatesChanged > 0)
			{
				await _smsTemplateWriter.CleanAndBulkInsertAsync(smsNoSqlEntities, DataSynchronizationPeriod.Min1);
				_logger.LogInformation("{cnt} {sqlName} entities updated.", smsTemplatesChanged, nameof(SmsTemplateMyNoSqlEntity));
			}

			if (smsTemplatesChanged > 0)
			{
				await _pushTemplateWriter.CleanAndBulkInsertAsync(pushNoSqlEntities, DataSynchronizationPeriod.Min1);
				_logger.LogInformation("{cnt} {sqlName} entities updated.", smsTemplatesChanged, nameof(PushTemplateNoSqlEntity));
			}

			return new DownloadGrpcResponse
			{
				Successful = true,
				TemplatesChanged = templatesChanged,
				SmsTemplatesChanged = smsTemplatesChanged,
				PushTemplatesChanged = pushTemplatesChanged
			};
		}
	}
}