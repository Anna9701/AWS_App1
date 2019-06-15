using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace AWS_FileProcessing {
    internal interface IRequestsHandler {
        Task ProcessMessage(Message message);
    }
}