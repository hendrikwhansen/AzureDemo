using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using EmployeeApi.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Azure.Identity;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailController> _logger;
    private readonly TelemetryClient _telemetryClient;

    public MailController(
        IConfiguration configuration, 
        ILogger<MailController> logger,
        TelemetryClient telemetryClient)
    {
        _configuration = configuration;
        _logger = logger;
        _telemetryClient = telemetryClient;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(MailRequest mailRequest)
    {
        using var operation = _telemetryClient.StartOperation<RequestTelemetry>("SendMail");
        
        try
        {
            var connectionString = _configuration.GetValue<string>("AzureServiceBus:ConnectionString")
                ?? throw new InvalidOperationException("Service Bus connection string not found.");
            var queueName = _configuration.GetValue<string>("AzureServiceBus:QueueName")
                ?? throw new InvalidOperationException("Service Bus queue name not found.");

            _telemetryClient.TrackEvent("MailRequestReceived", new Dictionary<string, string>
            {
                { "recipientAddress", mailRequest.RecipientAddress },
                { "subject", mailRequest.Subject }
            });

            await using var client = new ServiceBusClient(connectionString, new DefaultAzureCredential());
            await using var sender = client.CreateSender(queueName);

            var message = new ServiceBusMessage(JsonSerializer.Serialize(mailRequest))
            {
                ContentType = "application/json",
                Subject = mailRequest.Subject
            };

            await sender.SendMessageAsync(message);
            
            _telemetryClient.TrackEvent("MailRequestQueued", new Dictionary<string, string>
            {
                { "recipientAddress", mailRequest.RecipientAddress },
                { "messageId", message.MessageId }
            });

            _logger.LogInformation("Mail request sent to queue for recipient: {Recipient}", mailRequest.RecipientAddress);

            return Ok(new { message = "Mail request has been queued" });
        }
        catch (Exception ex)
        {
            _telemetryClient.TrackException(ex, new Dictionary<string, string>
            {
                { "recipientAddress", mailRequest.RecipientAddress },
                { "operation", "SendMail" }
            });
            
            _logger.LogError(ex, "Error sending mail request to queue");
            return StatusCode(500, new { message = "Error processing mail request" });
        }
    }
} 