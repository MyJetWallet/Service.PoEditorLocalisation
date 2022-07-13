using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.PoEditorLocalisation.Client;
using Service.PoEditorLocalisation.Grpc.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            var factory = new PoEditorLocalisationClientFactory("http://localhost:5001");
            var client = factory.GetLocalisationService();

            //var resp = await  client.ExportAsync(new ExportGrpcRequest(){Name = "Alex"});
            //Console.WriteLine(resp?.Message);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
