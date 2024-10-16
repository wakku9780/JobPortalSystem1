using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;


namespace JobPortalSystem.Models
{
    public class EmailService
    {

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Job Portal", "your_email@example.com")); // Change to your sender email
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.example.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("your_email@example.com", "your_email_password");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                // Log exception details for further troubleshooting
            }

        }

    }
}
