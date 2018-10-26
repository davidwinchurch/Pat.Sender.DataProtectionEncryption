﻿using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Pat.DataProtection;
using Pat.Sender.MessageGeneration;

namespace Pat.Sender.DataProtectionEncryption
{
    public class EncryptedMessageGenerator: IMessageGenerator
    {
        private readonly IDataProtector _dataProtector;

        public EncryptedMessageGenerator(DataProtectionConfiguration configuration)
        {
            var provider = DataProtection.DataProtectionProvider.Create(configuration);
            _dataProtector = provider.CreateProtector("PatLite");
        }
        
        public Message GenerateBrokeredMessage(object payload)
        {
            return GenerateMessage(payload);
        }

        public Message GenerateMessage(object payload)
        {
            var messageBody = JsonConvert.SerializeObject(payload);
            var protectedMessageBody = _dataProtector.Protect(messageBody);
            var message = new Message(Encoding.UTF8.GetBytes(protectedMessageBody));
            message.UserProperties.Add("Encrypted", true);
            return message;
        }
    }
}