using Confluent.Kafka;
using KafkaConfig;
using System;
using System.Threading;

namespace kafka_consumer_simples_csharp
{
    class Program
    {
        public static void Main()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = KafkaServerConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var c = new ConsumerBuilder<Ignore, string>(conf).Build();
            {
                c.Subscribe(KafkaTopicConfig.DefaultTopicName);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Lendo mensagem '{cr.Message.Value}' de: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Erro ao ler mensagem: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}
