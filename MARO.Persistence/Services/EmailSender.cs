using MailKit.Net.Smtp;
using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces;
using MimeKit;

namespace MARO.Persistence.Services
{
    public class EmailSender : IEmailSender
    {
        public string Email { get; set; } = null!;
        public string To { get; set; } = null!;
        public string From { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Subject { get; set; } = null!;

        public EmailSenderOptions Options { get; }

        public EmailSender(EmailSenderOptions senderOptions)
        {
            Options = senderOptions;
        }

        public async Task<string> SendAsync()
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Options.Name, Options.Username));
            emailMessage.To.Add(new MailboxAddress("", Email));
            emailMessage.Subject = Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Options.Host, Options.Port, false);
                await client.AuthenticateAsync(Options.Username, Options.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }

            return string.Empty;
        }
    }
}
