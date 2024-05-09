namespace TestClient;

public interface IEmailValidatorService
{
    bool ValidateEmail(string? email);
}