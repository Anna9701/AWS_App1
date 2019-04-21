using System;
using Amazon;
using Amazon.S3;
using AWS_S3_FilleProcessingLib;

namespace AWS_FileProcessing {
    internal class Program {
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;

        private const String BucketName = "filemanipulationsappstorage1";
        private const String KeyName = "SampleFile";
        private const String FilePath = @"D:\source\repos\polibuda\AWS_App1\README.md";

        private static void Main(String[] args) {
            Console.WriteLine("Hello World!");
            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                bucketFilesManager.UploadFileAsync(FilePath, BucketName).Wait();
            }
        }
    }
}