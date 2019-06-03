using System;

namespace AWS_SQSLibrary {
    public class MergeRequestMessage {
        public const String CommandIndicator = "Merge";
        private const String KeysSeparator = ";";
        public String FileAKey { get; }
        public String FileBKey { get; }

        public MergeRequestMessage(String fileBKey, String fileAKey) {
            FileBKey = fileBKey;
            FileAKey = fileAKey;
        }

        public MergeRequestMessage(String message) {
            message = message.Substring(CommandIndicator.Length).Trim();
            var split = message.Split(KeysSeparator);
            FileAKey = split[0];
            FileBKey = split[1];
        }

        public override String ToString() {
            return $"{CommandIndicator} {FileAKey}{KeysSeparator}{FileBKey}";
        }
    }
}