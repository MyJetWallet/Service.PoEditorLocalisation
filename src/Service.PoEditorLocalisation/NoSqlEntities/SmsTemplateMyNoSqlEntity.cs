using MyNoSqlServer.Abstractions;
using Service.SmsSender.Domain.Models;

namespace Service.PoEditorLocalisation.NoSqlEntities
{
	public class SmsTemplateMyNoSqlEntity : MyNoSqlDbEntity
	{
		public const string TableName = "myjetwallet-sms-template";

		public static string GeneratePartitionKey() => "Templates";

		public static string GenerateRowKey(string templateId) => templateId;

		public SmsTemplate Template { get; set; }

		public static SmsTemplateMyNoSqlEntity Create(SmsTemplate template)
		{
			return new SmsTemplateMyNoSqlEntity
			{
				PartitionKey = GeneratePartitionKey(),
				RowKey = GenerateRowKey(template.Id.ToString()),
				Template = template
			};
		}
	}
}