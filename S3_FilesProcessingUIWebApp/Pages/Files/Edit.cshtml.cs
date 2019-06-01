using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model;
using AWS_S3_FilleProcessingLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace S3_FilesProcessingUIWebApp.Pages.Files {
    public class EditModel : PageModel {
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUCentral1;
        private readonly String _bucketName;

        [BindProperty]
        public String FileToMergeWith { get; set; }
        
        public List<S3Object> FilesInBucket { get; set; }

        public EditModel(IConfiguration configuration) {
            _bucketName = configuration["BucketName"];
        }

        [HttpGet]
        public async Task OnGetAsync(String fileKey) {
            using (S3BucketFilesManager bucketFilesManager = new S3BucketFilesManager(BucketRegion)) {
                FilesInBucket = (await bucketFilesManager.ListFilesAsync(_bucketName)).Where(file => file.Key != fileKey).ToList();
            }

            TempData["fileKey"] = fileKey;
        }

        [HttpPost]
        public async Task<RedirectToPageResult> OnPostAsync() {
            var key = TempData["fileKey"];

            return RedirectToPage("/Files/Edit", key);
        }
    }
}