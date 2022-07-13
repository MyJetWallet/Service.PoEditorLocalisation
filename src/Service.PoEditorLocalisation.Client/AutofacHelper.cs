using Autofac;
using Service.PoEditorLocalisation.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.PoEditorLocalisation.Client
{
    public static class AutofacHelper
    {
        public static void RegisterPoEditorLocalisationClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new PoEditorLocalisationClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetLocalisationService()).As<ILocalisationService>().SingleInstance();
        }
    }
}
