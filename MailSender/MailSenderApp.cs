using TestClient;

namespace MailSender;

public class MailSenderApp
{
    private readonly IEmailValidatorService _emailValidatorService;
    private readonly IEmailService _emailService;

    public MailSenderApp(IEmailValidatorService emailValidatorService, IEmailService emailService)
    {
        _emailValidatorService = emailValidatorService;
        _emailService = emailService;
    }

    public async Task RunAsync()
    {
        bool isSent = false;

        //if something went wrong, the application would start again for retrying to send an email
        while (!isSent)
        {
            var emailDetails = AskEmailDetails();

            //I skipped the validation for other fields. In the real project they also need to be validated.
            var isSenderEmailValid = _emailValidatorService.ValidateEmail(emailDetails.Sender.Email);
            var isRecipientEmailValid = _emailValidatorService.ValidateEmail(emailDetails.Recipient.Email);
            var areEmailAddressesValid = isSenderEmailValid && isRecipientEmailValid;

            if (!areEmailAddressesValid)
            {
                Console.WriteLine("One of the email addresses is wrong");
                Console.WriteLine("Lets try again...");
                continue;
            }

            var response = await _emailService.SendAsync(emailDetails);
            Console.WriteLine("\nResponse: " + response?.Content);

            isSent = response.IsSuccessful;
            if (isSent)
            {
                Console.WriteLine("The message has been sent successfully!\n");
            }
        }
    }
    EmailDetails AskEmailDetails()
    {
        Console.WriteLine();

        Console.Write("Please, write the sender email: ");
        string? senderEmail = Console.ReadLine();

        Console.Write("Please, write the sender name: ");
        string? senderName = Console.ReadLine();

        Console.Write("Please, write the recipient email: ");
        string? recipientEmail = Console.ReadLine();

        Console.Write("Please, write the recipient name: ");
        string? recipientName = Console.ReadLine();

        Console.Write("Please, write the subject of the email: ");
        string? subject = Console.ReadLine();

        Console.Write("Please, write the content of the email: ");
        string? content = Console.ReadLine();

        Console.Write("Please, write the html content of the email (leave empty if there is not one): ");
        string? htmlContent = Console.ReadLine();

        var sender = new EmailUser(senderEmail, senderName);
        var recipient = new EmailUser(recipientEmail, recipientName);

        return new EmailDetails(sender, recipient, subject, content, htmlContent);
    }
}