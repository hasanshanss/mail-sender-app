using System.Net.Mail;

namespace TestClient;

public record EmailDetails(
    EmailUser Sender, 
    EmailUser Recipient, 
    string Subject = "(Empty Subject)", 
    string Text = "(empty)", 
    string Html = "");
public record EmailUser(string? Email, string Name);