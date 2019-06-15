using System;
using Amazon.SQS.Model;

namespace AWS_SQSLibrary {
    public class CloneRequestMessage : IRequestMessage {
        public const String CommandIndicator = "Clone";
        public String FileKey { get; }

        public CloneRequestMessage(String fileKey) {
            FileKey = fileKey;
        }

        private CloneRequestMessage(Message message) {
            FileKey = message.Body.Substring(CommandIndicator.Length).Trim();
        }

        public override String ToString() {
            return $"{CommandIndicator} {FileKey}";
        }

        public static explicit operator CloneRequestMessage(Message message) {
            if (message.Body.StartsWith(CommandIndicator)) {
                return new CloneRequestMessage(message);
            }
            throw new InvalidCastException();
        }
    }
}