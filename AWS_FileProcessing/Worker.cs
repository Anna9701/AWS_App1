using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using AWS_SQSLibrary;

namespace AWS_FileProcessing {
    internal class Worker {
        private readonly MergeRequestsHandler _requestsHandler;
        private readonly AwsSqsHandler _sqsHandler;

        public Worker(String bucketName, String queueUrl, RegionEndpoint bucketRegion) {
            _sqsHandler = new AwsSqsHandler(queueUrl, bucketRegion);
            _requestsHandler = new MergeRequestsHandler(bucketName, bucketRegion);
        }

        public async Task Run() {
            while (true) {
                var messages = await _sqsHandler.ReceiveMessage();
                foreach (var message in messages) {
                    await _requestsHandler.ProcessMessage(message);
                    HttpStatusCode deleteResult = await _sqsHandler.DeleteMessage(message);
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}