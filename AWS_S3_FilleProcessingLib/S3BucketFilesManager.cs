using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace AWS_S3_FilleProcessingLib {
    public class S3BucketFilesManager : IDisposable {
        private readonly RegionEndpoint _bucketRegion;
        private readonly IAmazonS3 _s3Client;

        public S3BucketFilesManager(RegionEndpoint bucketRegion) {
            _bucketRegion = bucketRegion;
            _s3Client = new AmazonS3Client(bucketRegion);
        }

        public void Dispose() {
            _s3Client.Dispose();
        }

        public async Task<List<S3Object>> ListFilesAsync(String bucketName) {
            ListObjectsResponse files = await _s3Client.ListObjectsAsync(bucketName);
            return files.S3Objects;
        }

        public async Task UploadFileAsync(String filePath, String bucketName, String keyName) {
            try {
                TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine($"Upload of {keyName} completed");
            }
            catch (AmazonS3Exception e) {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e) {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public async Task<String> DownloadFileAsync(String fileKey, String bucketName) {
            try {
                TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
                TransferUtilityDownloadRequest fileDownloadRequest = new TransferUtilityDownloadRequest {
                    Key = fileKey, BucketName = bucketName, FilePath = Path.GetTempPath() + fileKey
                };
                await fileTransferUtility.DownloadAsync(fileDownloadRequest);
                return fileDownloadRequest.FilePath;
            } catch (AmazonS3Exception e) {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            } catch (Exception e) {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
        }
    }
}