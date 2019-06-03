using System;
using System.Threading.Tasks;
using Amazon;

namespace AWS_FileProcessing {
    internal class Program {
        private const String BucketName = "filemanipulationsappstorage1";
        private const String QueueUrl = "https://sqs.eu-central-1.amazonaws.com/650110513798/FilesProcessingQueue";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;

        private static void Main(String[] args) {
            Worker worker = new Worker(BucketName, QueueUrl, BucketRegion);
            new TaskFactory().StartNew(async () => { await worker.Run(); });
            while (true) {}
        }
    }
}