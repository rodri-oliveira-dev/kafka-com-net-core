using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaConfig;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace kafka_producer_simples_csharp
{
    class Program
    {
        public static async Task Main()
        {
            await CriaTopicoCasoNaoExista();

            var config = new ProducerConfig { BootstrapServers = KafkaServerConfig.BootstrapServers };

            using var builder = new ProducerBuilder<string, string>(config).Build();
            {
                try
                {
                    while (true)
                    {
                        var dr = await builder.ProduceAsync(KafkaTopicConfig.DefaultTopicName, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = $"Valor: {Guid.NewGuid()}" });

                        Console.WriteLine($"Enviando '{dr.Value}' para '{dr.TopicPartitionOffset}'");

                        Thread.Sleep(2000);
                    }
                }
                catch (ProduceException<string, string> e)
                {
                    Console.WriteLine($"Facha no envio: {e.Error.Reason}");
                }
            }
        }

        private static async Task CriaTopicoCasoNaoExista()
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = KafkaServerConfig.BootstrapServers }).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new TopicSpecification[]
                    {
                        new TopicSpecification { Name = KafkaTopicConfig.DefaultTopicName, ReplicationFactor = KafkaTopicConfig.ReplicationFactor, NumPartitions = KafkaTopicConfig.NumPartitions }
                    });
                    Console.WriteLine($"Tópico criado");
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine($"Não foi possivel criar o Tóppico: {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }
    }
}
