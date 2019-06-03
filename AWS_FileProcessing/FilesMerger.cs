using System;
using System.Collections.Generic;
using System.IO;

namespace AWS_FileProcessing {
    internal class FilesMerger {
        public void MergeFiles(String outputFilePath, List<String> files) {
            using (FileStream outputStream = File.Create(outputFilePath)) {
                foreach (String inputFilePath in files)
                    using (FileStream inputStream = File.OpenRead(inputFilePath)) {
                        inputStream.CopyTo(outputStream);
                    }
            }
        }
    }
}