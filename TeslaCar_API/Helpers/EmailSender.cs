using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace TeslaCar_API.Helpers
{
    // 250. В папке Helpers добавить новый класс EmailSender
    public class EmailSender : IEmailSender
    {
        // 251.1 Перейти на https://github.com/mailjet/mailjet-apiv3-dotnet раздел Make your first call скопировать метод static async Task RunAsync() в новый класс
        // 251.2 Установить в API проект пакет mailjet
        //static async Task RunAsync()
        //{
        //    MailjetClient client = new MailjetClient(
        //       Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"),
        //       Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"));

        //    MailjetRequest request = new MailjetRequest
        //    {
        //        Resource = Send.Resource,
        //    };

        // // construct your email with builder
        //    var email = new TransactionalEmailBuilder()
        //        .WithFrom(new SendContact("from@test.com"))
        //        .WithSubject("Test subject")
        //        .WithHtmlPart("<h1>Header</h1>")
        //        .WithTo(new SendContact("to@test.com"))
        //        .Build();

        //    // invoke API to send email
        //    var response = await client.SendTransactionalEmailAsync(email);

        //    // check response
        //    //Assert.AreEqual(1, response.Messages.Length);
        //}

        private readonly MailJetSettings _mailJetSettings;
        public EmailSender(IOptions<MailJetSettings> mailJetSettings)
        {
            _mailJetSettings = mailJetSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient(
               _mailJetSettings.PublicKey,
               _mailJetSettings.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            };

            // construct your email with builder
            var emailLetter = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_mailJetSettings.Email))
                .WithSubject(subject)
                .WithHtmlPart(htmlMessage)
                .WithTo(new SendContact(email))
                .Build();

            // invoke API to send email
            var response = await client.SendTransactionalEmailAsync(emailLetter);
        }
    }
}
