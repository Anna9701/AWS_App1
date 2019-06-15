using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const String SuccessMergeRequest = "Your merge request is being processed...";
        private const String FailureMergeRequest = "Your merge request failed with code {0}.";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;
        private readonly String _bucketName;

        public IndexModel(IConfiguration configuration) {
            _bucketName = configuration["BucketName"];
        }

        [BindProperty] public String MessageContent { get; set; }

        [BindProperty] public Dictionary<String, Boolean> S3KeysToClone { get; set; } = new Dictionary<String, Boolean>();

        public IList<S3Object> FilesInBucket { get; set; }


        public async Task OnGetAsync() {
            FilesInBucket = await GetFilesFromBucket();

            foreach (S3Object file in FilesInBucket) S3KeysToClone[file.Key] = false;

            if (TempData.ContainsKey("MergeResult")) {
                Int32 code = Int32.Parse(TempData["MergeResult"].ToString());
                MessageContent = code >= 200 && code < 300 ? SuccessMergeRequest : String.Format(FailureMergeRequest, code);
            }
        }

        private async Task<List<S3Object>> GetFilesFromBucket() {
            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                return await bucketFilesManager.ListFilesAsync(_bucketName);
            }
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file) {
            String filePath = Path.GetTempFileName();
            using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                bucketFilesManager.UploadFileAsync(filePath, _bucketName, file.FileName);
            }

            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnPostCloneAsync() {
            IList<S3Object> s3ObjectsInBucket = await GetFilesFromBucket();
            IList<S3Object> filesToClone = new List<S3Object>();
            foreach ((String fileName, Boolean toClone) in S3KeysToClone)
                if (toClone) {
                    S3Object fileToClone = s3ObjectsInBucket.SingleOrDefault(file => file.Key == fileName);
                    filesToClone.Add(fileToClone);
                }

            return RedirectToPage("./Index");
        }
    }
}