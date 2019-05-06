using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using AWS_S3_FilleProcessingLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace S3_FilesProcessingUIWebApp.Pages.Files {
    public class IndexModel : PageModel {
        private readonly String _bucketName;
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;

        public IndexModel(IConfiguration configuration) {
            _bucketName = configuration["BucketName"];
        }

        public void OnGet() {
        }

        public async Task OnPostAsync(IFormFile file) {
            String filePath = Path.GetTempFileName();
            using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }
            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                bucketFilesManager.UploadFileAsync(filePath, _bucketName, file.FileName);
            }
        }
    }
}