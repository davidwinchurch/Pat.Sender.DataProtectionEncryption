using System.Collections;
using System.Linq;
using Microsoft.ServiceBus.Messaging;
using System;

namespace PB.ITOps.Messaging.PatSender.Extensions
{
    public static class BrokeredMessageExtensions
    {
        public static BrokeredMessage PopulateCorrelationId(this BrokeredMessage message, string correlationId)
        {
            if (!string.IsNullOrEmpty(correlationId))
            {
                message.Properties["PBCorrelationId"] = correlationId;
            }

            return message;
        }

        public static long GetSize(this BrokeredMessage message)
        {
            long estimatedSize = 61;
            int minimumFieldSize = 8;

            estimatedSize += message.ContentType.Length;
            foreach (var propertyPair in message.Properties.AsEnumerable())
            {
                estimatedSize += propertyPair.Key.Length + 5;
                if (propertyPair.Value != null)
                {
                    estimatedSize += Math.Max(minimumFieldSize, propertyPair.Value.ToString().Length);
                }
            }
            estimatedSize += message.Size;
            return estimatedSize;
        }
    }
}