using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcTest;
using System.ServiceModel;

namespace GrpcTest.Services
{
	[ServiceContract]
	public interface IGreeter
	{
		Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context);
	}

	public class GreeterService : Greeter.GreeterBase, IGreeter
	{
		private readonly ILogger<GreeterService> _logger;
		public GreeterService(ILogger<GreeterService> logger)
		{
			_logger = logger;
		}

		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
		{
			return Task.FromResult(new HelloReply
			{
				Message = "Hello " + request.Name
			});
		}


		public override async Task BunchOfHellos(HelloRequest request,
			IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
		{
			for (var i = 0; i < 20; i++)
			{
				await responseStream.WriteAsync(new HelloReply
				{
					Message = $"Hello {request.Name} times {i}!"
				}); 
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}
	}
}
