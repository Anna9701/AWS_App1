using System;

namespace AWS_SQSLibrary {
    public class MergeRequestMessage {
        public const String CommandIndicator = "Merge";
        private const String KeysSeparator = ";";
        private readonly String _fileAKey;
        private readonly String _fileBKey;

        public MergeRequestMessage(String fileBKey, String fileAKey) {
            _fileBKey = fileBKey;
            _fileAKey = fileAKey;
        }

        public override String ToString() {
            return $"{CommandIndicator} {_fileAKey}{KeysSeparator}{_fileBKey}";
        }
    }
}