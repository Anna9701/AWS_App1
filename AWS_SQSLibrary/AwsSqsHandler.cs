using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWS_SQSLibrary
{
    public class AwsSqsHandler {
        private readonly String _queueUrl;
        private readonly AmazonSQSClient _client;

        public AwsSqsHandler(String queueUrl, RegionEndpoint bucketRegion) {
            _queueUrl = queueUrl;
            _client = new AmazonSQSClient(bucketRegion);
        }

        public async Task<List<Message>> ReceiveMessage() {
            ReceiveMessageResponse message = await _client.ReceiveMessageAsync(_queueUrl);
            return message.Messages;
        }

        public async Task<HttpStatusCode> SendMessage(String messageContent) {
            SendMessageResponse message = await _client.SendMessageAsync(_queueUrl, messageContent);
            return message.HttpStatusCode;
        }

        public async Task<HttpStatusCode> DeleteMessage(Message messageToRemove) {
            DeleteMessageResponse deleteMessageResponse = await _client.DeleteMessageAsync(_queueUrl, messageToRemove.ReceiptHandle);
            return deleteMessageResponse.HttpStatusCode;
        }
    }
}
