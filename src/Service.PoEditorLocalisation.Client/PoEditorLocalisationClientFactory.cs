using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.PoEditorLocalisation.Grpc;

namespace Service.PoEditorLocalisation.Client
{
    [UsedImplicitly]
    public class PoEditorLocalisationClientFactory: MyGrpcClientFactory
    {
        public PoEditorLocalisationClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IPoEditorLocalisationService GetLocalisationService() => CreateGrpcService<IPoEditorLocalisationService>();
    }
}
