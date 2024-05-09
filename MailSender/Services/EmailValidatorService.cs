using System.Text.RegularExpressions;

namespace TestClient;

public class EmailValidatorService : IEmailValidatorService
{
    public EmailValidatorService()
    {
        
    }
    public bool ValidateEmail(string? email)
    {
        if (email == null)
        {
            return false;
        }
        
        string emailPattern = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
        return  Regex.IsMatch(email, emailPattern);
    }
}