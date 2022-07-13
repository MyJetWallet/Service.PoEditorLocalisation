using System.Runtime.Serialization;

namespace Service.PoEditorLocalisation.Grpc.Models
{
    [DataContract]
    public class ExportGrpcRequest
    {
        [DataMember(Order = 1)]
        public string Lang { get; set; }
    }
}