using System;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using AWS_S3_FilleProcessingLib;

namespace AWS_FileProcessing {
    internal class Program {
        private const String BucketName = "filemanipulationsappstorage1";
        private const String KeyName = "SampleFile";
        private const String FilePath = @"D:\source\repos\polibuda\AWS_App1\README.md";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;

        private static void Main(String[] args) {
            Console.WriteLine("Hello World!");
            AmazonSQSConfig amazonSQSConfig = new AmazonSQSConfig {ServiceURL = "https://sqs.eu-central-1.amazonaws.com/" };
            AmazonSQSClient client = new AmazonSQSClient(BucketRegion);
            var x = client.ListQueuesAsync("FilesProcessingQueue").Result;
        }
    }
}