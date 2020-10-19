using System;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcSample;

namespace GrpcSampleServer
{
    public class GreeterService : Greeter.GreeterBase
    {
        private static HelloReply _reply;

        static GreeterService()
        {
            _reply = new HelloReply();
            for (var i = 0; i < 1000; i++)
            {
                _reply.Payload.Add($"{DateTime.Now.Ticks}");
            }
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task SayHelloBiDi(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var helloReply = new HelloReply
                {
                    Message = "Hello " + requestStream.Current.Name
                };

                await responseStream.WriteAsync(helloReply);
            }
        }
    }
}
