﻿using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using PB.ITOps.Messaging.PatSender.Extensions;
using System;
using System.Threading.Tasks;

namespace PB.ITOps.Messaging.PatSender.Legacy
{
    public class LegacyMessagePublisher : ILegacyMessagePublisher
    {
        private readonly IMessageSender _messageSender;
        private readonly string _correlationId;

        public LegacyMessagePublisher(IMessageSender messageSender, string correlationId = null)
        {
            _messageSender = messageSender;
            _correlationId = correlationId;
        }

        public async Task PublishLegacyMessage<TLegacyMessage>(TLegacyMessage message, string legacyMessageType, string legacyContentType) where TLegacyMessage : class
        {
            var brokeredMessage = GenerateMessage(message, legacyMessageType, legacyContentType);
            await _messageSender.SendMessages(new[] { brokeredMessage });
        }

        private BrokeredMessage GenerateMessage(object message, string legacyMessageType, string legacyContentType)
        {
            var brokeredMessage = new BrokeredMessage(JsonConvert.SerializeObject(message))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = legacyContentType
            };

            brokeredMessage.Properties["MessageType"] = legacyMessageType;
            brokeredMessage.PopulateCorrelationId(_correlationId);

            return brokeredMessage;
        }
    }
}
