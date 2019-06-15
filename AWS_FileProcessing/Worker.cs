using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS.Model;
using AWS_SQSLibrary;

namespace AWS_FileProcessing {
    internal class Worker {
        private readonly List<IRequestsHandler> _requestsHandlers = new List<IRequestsHandler>();
        private readonly AwsSqsHandler _sqsHandler;

        public Worker(String bucketName, String queueUrl, RegionEndpoint bucketRegion) {
            _sqsHandler = new AwsSqsHandler(queueUrl, bucketRegion);
            _requestsHandlers.Add(new MergeRequestsHandler(bucketName, bucketRegion));
            _requestsHandlers.Add(new CloneRequestsHandler(bucketName, bucketRegion));
        }

        public async Task Run() {
            while (true) {
                var messages = await _sqsHandler.ReceiveMessage();
                foreach (Message message in messages) {
                    _requestsHandlers.ForEach(handler => handler.ProcessMessage(message));
                    _ = _sqsHandler.DeleteMessage(message);
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}