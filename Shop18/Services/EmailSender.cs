﻿using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop18.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configiration;

        private MailJetSettings _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configiration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configiration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey)
            {
                Version = ApiVersion.V3_1
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            }
            .Property(Send.Messages, new JArray {
             new JObject {
              {
               "From",
               new JObject {
                {"Email", "trofimchuk.an@gmail.com"},
                {"Name", "Online Shop"}
               }
              }, {
               "To",
               new JArray {
                new JObject {
                 {
                  "Email",
                  email
                 }, {
                  "Name",
                  "Customer"
                 }
                }
               }
              }, {
               "Subject",
               subject
              },{
               "HTMLPart",
               body
              }
             }
            });
            await client.PostAsync(request);
        }
    }
}
