﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Butterfly.Core.Notify;
using NLog;

namespace Butterfly.Aws {
    public class AwsSesEmailNotifyMessageSender : BaseNotifyMessageSender {

        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override async Task<string> DoSendAsync(string from, string to, string subject, string bodyText, string bodyHtml) {
            logger.Debug($"DoSendAsync():from={from},to={to},subject={subject}");

            Destination destination = new Destination {
                ToAddresses = (new List<string>() { to })
            };

            Content subjectContent = new Content(subject);
            Content bodyContent = new Content(bodyText);
            Body myBody = new Body(bodyContent);
            if (!string.IsNullOrEmpty(bodyHtml)) myBody.Html = new Content(bodyHtml);

            Message message = new Message(subjectContent, myBody);
            SendEmailRequest sendEmailRequest = new SendEmailRequest(from, destination, message);

            AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(Amazon.RegionEndpoint.USEast1);

            logger.Debug($"DoSendAsync():client.SendEmailAsync()");
            SendEmailResponse sendEmailResponse = await client.SendEmailAsync(sendEmailRequest);

            logger.Debug($"DoSendAsync():client.SendEmailAsync():sendEmailResponse.MessageId={sendEmailResponse.MessageId}");
            return sendEmailResponse.MessageId;

            /*
            try {
                logger.Debug($"DoSendAsync():client.SendEmailAsync()");
                SendEmailResponse sendEmailResponse = await client.SendEmailAsync(sendEmailRequest);
                return sendEmailResponse.MessageId;
            }
            catch (Exception ex) {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
            */
        }

    }
}

