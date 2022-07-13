using System.Collections.Generic;
using System.Linq;
using MyNoSqlServer.Abstractions;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Domain.Models.Enums;

namespace Service.PoEditorLocalisation.NoSqlEntities
{
	public class PushTemplateNoSqlEntity : MyNoSqlDbEntity
	{
		public const string TableName = "myjetwallet-pushnotification-templates";

		public static string GeneratePartitionKey() => "Templates";

		public static string GenerateRowKey(NotificationTypeEnum type) => type.ToString();

		public NotificationTypeEnum Type { get; set; }

		public string DefaultBrand { get; set; }

		public string DefaultLang { get; set; }

		public Dictionary<string, string> BodiesSerializable { get; set; }

		public List<string> Params { get; set; }

		public static PushTemplateNoSqlEntity Create(NotificationTemplate template) =>
			new()
			{
				PartitionKey = GeneratePartitionKey(),
				RowKey = GenerateRowKey(template.Type),
				Type = template.Type,
				DefaultBrand = template.DefaultBrand,
				DefaultLang = template.DefaultLang,
				Params = template.Params,
				BodiesSerializable = template.Bodies.ToDictionary((pair => $"{pair.Key.brand};-;{pair.Key.lang}"), pair => $"{pair.Value.topic};-;{pair.Value.body}")
			};
	}
}