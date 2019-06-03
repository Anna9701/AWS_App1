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
    internal class MergeRequestsHandler {
        private readonly S3BucketFilesManager _bucketFilesManager;
        private readonly String _bucketName;
        private readonly FilesMerger _filesMerger = new FilesMerger();

        public MergeRequestsHandler(String bucketName, RegionEndpoint bucketRegion) {
            _bucketFilesManager = new S3BucketFilesManager(bucketRegion);
            _bucketName = bucketName;
        }

        public async Task ProcessMessage(Message message) {
                MergeRequestMessage request = new MergeRequestMessage(message.Body);
                var files = new List<String> {
                    await _bucketFilesManager.DownloadFileAsync(request.FileAKey, _bucketName),
                    await _bucketFilesManager.DownloadFileAsync(request.FileBKey, _bucketName)
                };
                String outputFilePath = Path.GetTempPath() + request.FileAKey + request.FileBKey;
                _filesMerger.MergeFiles(outputFilePath, files);

                await _bucketFilesManager.UploadFileAsync(outputFilePath, _bucketName, request.FileAKey + request.FileBKey);
            
        }
    }
}