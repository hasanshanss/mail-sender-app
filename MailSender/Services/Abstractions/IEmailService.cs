using RestSharp;

namespace TestClient;

public interface IEmailService
{
    public Task<RestResponse> SendAsync(EmailDetails ep);
}

