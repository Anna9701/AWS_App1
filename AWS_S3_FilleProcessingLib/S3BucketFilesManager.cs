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

                // Option 3. Upload data from a type of System.IO.Stream.
                using (FileStream fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, keyName);
                }

                Console.WriteLine("Upload 3 completed");

                // Option 4. Specify advanced settings.
                TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest {
                    BucketName = bucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                Console.WriteLine("Upload 4 completed");
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