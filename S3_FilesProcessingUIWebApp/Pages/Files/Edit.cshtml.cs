using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model;
using AWS_S3_FilleProcessingLib;
using AWS_SQSLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace S3_FilesProcessingUIWebApp.Pages.Files {
    public class EditModel : PageModel {
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;
        private readonly String _bucketName;
        private readonly AwsSqsHandler _sqsHandler;
        
        [BindProperty]
        public String CurrentFileKey { get; set; }

        [BindProperty]
        public String FileToMergeWith { get; set; }
        
        public List<S3Object> FilesInBucket { get; set; }

        public EditModel(IConfiguration configuration) {
            _bucketName = configuration["BucketName"];
            _sqsHandler = new AwsSqsHandler(configuration["QueueUrl"], BucketRegion);
        }

        [HttpGet]
        public async Task OnGetAsync(String fileKey) {
            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                FilesInBucket = (await bucketFilesManager.ListFilesAsync(_bucketName)).Where(file => file.Key != fileKey).ToList();
            }
            CurrentFileKey = fileKey;
        }

        [HttpPost]
        public async Task<RedirectToPageResult> OnPostAsync() {
            HttpStatusCode result = await _sqsHandler.SendMessage(new MergeRequestMessage(FileToMergeWith, CurrentFileKey).ToString());
            TempData["MergeResult"] = result;
            return RedirectToPage("./Index");
        }
    }
}