using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using System;

namespace GrpcClient
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Calling a gRpc Service!");
            Console.WriteLine("hit enter to call the service...");
            Console.ReadLine();
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            Console.Write("Enter Your Name: ");
            var name = Console.ReadLine();
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            Console.WriteLine($"This service said {reply.Message}");



            Console.Write("Where do you want to go today?");
            var address = Console.ReadLine();
            var client2 = new TurnByTurn.TurnByTurnClient(channel);
            var streamingResponse = client2.StartGuidance(new GuidanceRequest { Address = address });
            await foreach(var step in streamingResponse.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Turn {step.Direction} at {step.Road}");
            }

            Console.WriteLine("You have arrived at " + address);
        }
    }
}
