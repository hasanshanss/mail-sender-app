using MailSender;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestClient;
using static ServiceExtensions;

using IHost host = AddServices().Build();
AddExceptionHandler();

var mailApp = host.Services.GetService<MailSenderApp>();
await mailApp.RunAsync();

await host.RunAsync();

HostApplicationBuilder AddServices()
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddTransient<IEmailService, EmailService>();
    builder.Services.AddTransient<IEmailValidatorService, EmailValidatorService>();
    builder.Services.AddSingleton<MailSenderApp>();
    return builder;
}

#region web-version
// This could be a web implementation through .NET Minimal API. Also, possible to write it with Controllers.

// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();
//
// //Instead of sending EmailDetails, the DTO should be used and the be mapped. But for the brevity, I skipped that part.
// app.MapPost("/send", async (EmailDetails emailDetails, IEmailValidatorService emailValidator, IEmailService emailService) =>
// {
//     
//     //validate here first...
//
//     await emailService.SendAsync(emailDetails);
// });
//
// app.Run();
#endregion