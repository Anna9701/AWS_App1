using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using AWS_S3_FilleProcessingLib;
using AWS_SQSLibrary;

namespace AWS_FileProcessing {
    internal class Program {
        private const String BucketName = "filemanipulationsappstorage1";
        private const String KeyName = "SampleFile";
        private const String FilePath = @"D:\source\repos\polibuda\AWS_App1\README.md";
        private const String QueueUrl = "https://sqs.eu-central-1.amazonaws.com/650110513798/FilesProcessingQueue";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;
        
        private static void Main(String[] args) {
            AwsSqsHandler sqsHandler = new AwsSqsHandler(QueueUrl, BucketRegion);
            new TaskFactory().StartNew(async () => {
                while (true) {
                    var messages = await sqsHandler.ReceiveMessage();
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await sqsHandler.DeleteMessage(messages[0]);
                }
            });
            Console.ReadLine();
        }


    }
}