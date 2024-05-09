using Moq;
using RestSharp;
using TestClient;

namespace EmailSenderTest
{
    // Fixture class
    public class EmailValidatorFixture : IDisposable
    {
        public IEmailValidatorService EmailValidator { get; private set; }

        public EmailValidatorFixture()
        {
            EmailValidator = new EmailValidatorService();
        }

        public void Dispose()
        {
            // Clean up resources if needed
            EmailValidator = null;
        }
    }
    public class EmailSenderTests : IClassFixture<EmailValidatorFixture>
    {
        private readonly EmailValidatorFixture _emailValidatorFixture;

        public EmailSenderTests(EmailValidatorFixture emailValidatorFixture)
        {
            _emailValidatorFixture = emailValidatorFixture;
        }
        
        [Theory]
        [MemberData(nameof(MailSuccessfulTestData))]
        public async Task IfEmailIsValid_ShouldBeSent(EmailDetails emailDetails)
        {
            
            //Arrange
            var mockMailService = new Mock<IEmailService>();
            var successfulResponse = new RestResponse()
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            mockMailService.Setup(m => m.SendAsync(emailDetails)).ReturnsAsync(successfulResponse);
            
            //Act
            var isSenderEmailValid = _emailValidatorFixture.EmailValidator.ValidateEmail(emailDetails.Sender.Email);
            var isRecipientEmailValid = _emailValidatorFixture.EmailValidator.ValidateEmail(emailDetails.Recipient.Email);
            var areEmailAddressesValid = isSenderEmailValid && isRecipientEmailValid;
            
            var isSuccessResponse = false;
            if(areEmailAddressesValid)
            {
                var response = await mockMailService.Object.SendAsync(emailDetails);
                isSuccessResponse = response.IsSuccessful;
            }
            
            //Assert
            Assert.True(isSuccessResponse);
        }
        
        [Theory]
        [MemberData(nameof(MailErrorTestData))]
        public async Task IfEmailIsWrong_ShouldNotBeSent(EmailDetails emailDetails)
        {
            //Arrange
            var mockMailService = new Mock<IEmailService>();
            var errorResponse = new RestResponse()
            {
                IsSuccessStatusCode = false,
                ResponseStatus = ResponseStatus.Error
            };

            mockMailService.Setup(m => m.SendAsync(emailDetails)).ReturnsAsync(errorResponse);
            
            //Act
            var isSenderEmailValid = _emailValidatorFixture.EmailValidator.ValidateEmail(emailDetails.Sender.Email);
            var isRecipientEmailValid = _emailValidatorFixture.EmailValidator.ValidateEmail(emailDetails.Recipient.Email);
            var areEmailAddressesValid = isSenderEmailValid && isRecipientEmailValid;
            
            var isSuccessResponse = false;
            if(areEmailAddressesValid)
            {
                var response = await mockMailService.Object.SendAsync(emailDetails);
                isSuccessResponse = response.IsSuccessful;
            }
            
            //Assert
            Assert.False(isSuccessResponse);
        }
        
        
        public static IEnumerable<object[]> MailSuccessfulTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new EmailDetails(
                        new EmailUser("mailtrap@demomailtrap.com", "DemoUser"),
                        new EmailUser("hasan.suleymanli@gmail.com", "Hasan"),
                        "Thanks!",
                        "Thank you for applying",
                        ""
                    )
                },
                new object[]
                {
                    new EmailDetails(
                        new EmailUser("mailtrap@demomailtrap.com", "DemoUser"),
                        new EmailUser("hasan.suleymanli@gmail.com", "Hasan"),
                        "Thanks!",
                        "Thank you for applying",
                        "<html><h1>HELLO WORLD</h1></html>"
                    )
                }
            };
        
        public static IEnumerable<object[]> MailErrorTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new EmailDetails(
                        new EmailUser("mailtrapdemomailtrap.com", "DemoUser"),
                        new EmailUser("hasan.suleymanligmail.com", "Hasan"),
                        "Thanks!",
                        "Thank you for applying",
                        "")
                },
                new object[]
                {
                    new EmailDetails(
                        new EmailUser(null, "DemoUser"),
                        new EmailUser(null, "Hasan"),
                        "Thanks!",
                        "Thank you for applying",
                        ""
                    )
                },
            };
    }
}
