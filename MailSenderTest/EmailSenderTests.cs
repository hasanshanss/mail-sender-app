using Moq;
using RestSharp;
using TestClient;

namespace MailSenderTest
{
    public class EmailSenderTests
    {
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
            var emailValidatorService = new EmailValidatorService();

            //Act
            var isSenderEmailValid = emailValidatorService.ValidateEmail(emailDetails.Sender.Email);
            var isRecipientEmailValid = emailValidatorService.ValidateEmail(emailDetails.Recipient.Email);
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
            var emailValidatorService = new EmailValidatorService();
            
            //Act
            var isSenderEmailValid = emailValidatorService.ValidateEmail(emailDetails.Sender.Email);
            var isRecipientEmailValid = emailValidatorService.ValidateEmail(emailDetails.Recipient.Email);
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
