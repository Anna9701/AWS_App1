using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model;
using AWS_S3_FilleProcessingLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace S3_FilesProcessingUIWebApp.Pages.Files {
    public class IndexModel : PageModel {
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;
        private readonly String _bucketName;

        private const String SuccessMergeRequest = "Your merge request is being processed...";
        private const String FailureMergeRequest = "Your merge request failed with code {0}.";

        [BindProperty] public String MessageContent { get; set; }


        public IndexModel(IConfiguration configuration) {
            _bucketName = configuration["BucketName"];
        }

        public List<S3Object> FilesInBucket { get; set; }

        public async Task OnGetAsync() {
            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                FilesInBucket = await bucketFilesManager.ListFilesAsync(_bucketName);
            }

            if (TempData.ContainsKey("MergeResult")) {
                var code = Int32.Parse(TempData["MergeResult"].ToString());
                MessageContent = code >= 200 && code < 300 ? SuccessMergeRequest : string.Format(FailureMergeRequest, code);  
            }
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