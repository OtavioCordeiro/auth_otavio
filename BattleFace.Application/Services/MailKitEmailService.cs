using BattleFace.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public MailKitEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var smtpSection = _configuration.GetSection("SmtpSettings");

            // Criação da mensagem com MimeKit
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpSection["FromName"], smtpSection["From"]));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body,  // Corpo HTML do e-mail
                TextBody = body   // Corpo de texto alternativo
            };

            message.Body = builder.ToMessageBody();

            // Conectando e enviando o e-mail com MailKit
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpSection["Host"], int.Parse(smtpSection["Port"] ?? "587"), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpSection["Username"], smtpSection["Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
