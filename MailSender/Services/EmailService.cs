using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.RegularExpressions;
using RestSharp;
using Microsoft.Extensions.Configuration;

namespace TestClient;

public class EmailService : IEmailService
{
    private IConfiguration Configuration;

    public EmailService()
    {
        
    }
    
    public EmailService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public async Task<RestResponse> SendAsync(EmailDetails emailDetails)
    {
        var client = new RestClient(Configuration["MailService:ApiUrl"]);
        var request = new RestRequest("/send", RestSharp.Method.Post);
        var apiKey = $"Bearer {Configuration["MailService:ApiKey"]}";
        
        request.AddHeader("Authorization", apiKey);
        request.AddHeader("Content-Type", "application/json");
        request.AddParameter("application/json", GetEmailParameters(emailDetails), ParameterType.RequestBody);
            
        RestResponse response = await client.ExecuteAsync<RestResponse>(request);
        return response;
    }

   
    private string GetEmailParameters(EmailDetails emailDetails)
    {
        var to = new { email = emailDetails.Recipient.Email, name = emailDetails.Recipient.Name };
        var from = new { email = emailDetails.Sender.Email, name = emailDetails.Sender.Name };
    
        var emailParameters = new {
            from,
            to = new[] { to },
            subject = emailDetails.Subject,
            text = emailDetails.Text,
            html = emailDetails.Html
        };

        return JsonSerializer.Serialize(emailParameters);
    }
    
}