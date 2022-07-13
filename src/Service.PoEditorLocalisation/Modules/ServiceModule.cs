using Autofac;
using MyJetWallet.Sdk.NoSql;
using Service.MessageTemplates.Domain.Models.NoSql;
using Service.PoEditorLocalisation.NoSqlEntities;
using Service.PoEditorLocalisation.Services;

namespace Service.PoEditorLocalisation.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<PoEditorSender>().AsImplementedInterfaces().SingleInstance();

			builder.RegisterMyNoSqlWriter<TemplateNoSqlEntity>(() => Program.Settings.MyNoSqlWriterUrl, TemplateNoSqlEntity.TableName);
			builder.RegisterMyNoSqlWriter<SmsTemplateMyNoSqlEntity>(() => Program.Settings.MyNoSqlWriterUrl, SmsTemplateMyNoSqlEntity.TableName);
			builder.RegisterMyNoSqlWriter<PushTemplateNoSqlEntity>(() => Program.Settings.MyNoSqlWriterUrl, PushTemplateNoSqlEntity.TableName);
		}
	}
}