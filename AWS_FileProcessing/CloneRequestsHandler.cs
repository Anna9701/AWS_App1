using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS.Model;
using AWS_S3_FilleProcessingLib;
using AWS_SQSLibrary;

namespace AWS_FileProcessing {
    internal class CloneRequestsHandler : IRequestsHandler {
        private readonly S3BucketFilesManager _bucketFilesManager;
        private readonly String _bucketName;

        public CloneRequestsHandler(String bucketName, RegionEndpoint bucketRegion) {
            _bucketFilesManager = new S3BucketFilesManager(bucketRegion);
            _bucketName = bucketName;
        }

        public async Task ProcessMessage(Message message) {
            CloneRequestMessage request = (CloneRequestMessage) message;
            if (request == null) {
                return;
            }

            String filePath = await _bucketFilesManager.DownloadFileAsync(request.FileKey, _bucketName);
            String cloneKeyName = $"{Path.GetFileNameWithoutExtension(filePath)}_cloned{DateTime.Today.ToShortDateString()}{Path.GetExtension(filePath)}";

            _ = _bucketFilesManager.UploadFileAsync(filePath, _bucketName, cloneKeyName);
        }
        
    }
}