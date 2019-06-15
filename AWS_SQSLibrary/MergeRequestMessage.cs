using System;
using Amazon.SQS.Model;

namespace AWS_SQSLibrary {
    public class MergeRequestMessage : IRequestMessage {
        public const String CommandIndicator = "Merge";
        private const String KeysSeparator = ";";
        public String FileAKey { get; }
        public String FileBKey { get; }

        public MergeRequestMessage(String fileBKey, String fileAKey) {
            FileBKey = fileBKey;
            FileAKey = fileAKey;
        }

        private MergeRequestMessage(String message) {
            message = message.Substring(CommandIndicator.Length).Trim();
            var split = message.Split(KeysSeparator);
            FileAKey = split[0];
            FileBKey = split[1];
        }

        public override String ToString() {
            return $"{CommandIndicator} {FileAKey}{KeysSeparator}{FileBKey}";
        }

        public static explicit operator MergeRequestMessage(Message message) {
            if (message.Body.StartsWith(CommandIndicator, StringComparison.InvariantCultureIgnoreCase)) {
                return new MergeRequestMessage(message.Body);
            }

            throw new InvalidCastException();
        }
    }
}