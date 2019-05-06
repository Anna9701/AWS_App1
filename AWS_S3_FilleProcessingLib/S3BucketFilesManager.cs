using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace AWS_S3_FilleProcessingLib {
    public class S3BucketFilesManager : IDisposable {
        private readonly IAmazonS3 _s3Client;
        private readonly RegionEndpoint _bucketRegion;

        public S3BucketFilesManager(RegionEndpoint bucketRegion) {
            _bucketRegion = bucketRegion;
            _s3Client = new AmazonS3Client(bucketRegion);
        }

        public void Dispose() {
            _s3Client.Dispose();
        }

        public async Task UploadFileAsync(String filePath, String bucketName, String keyName) {
            try {
                TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine("Upload 2 completed");
            } catch (AmazonS3Exception e) {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            } catch (Exception e) {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public async Task UploadFileAsync(String filePath, String bucketName) {
            try {
                TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, bucketName);
                Console.WriteLine("Upload 1 completed");
            } catch (AmazonS3Exception e) {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            } catch (Exception e) {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}