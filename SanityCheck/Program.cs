using System;
using System.Threading.Tasks;
using NServiceBus;

namespace SanityCheck
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "sanity";
            var endpointConfiguration = new EndpointConfiguration("APIEvents.Endpoint");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
