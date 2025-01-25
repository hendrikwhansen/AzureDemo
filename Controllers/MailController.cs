using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailController> _logger;

    public MailController(IConfiguration configuration, ILogger<MailController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(MailRequest mailRequest)
    {
        try
        {
            var connectionString = _configuration.GetValue<string>("AzureServiceBus:ConnectionString")
                ?? throw new InvalidOperationException("Service Bus connection string not found.");
            var queueName = _configuration.GetValue<string>("AzureServiceBus:QueueName")
                ?? throw new InvalidOperationException("Service Bus queue name not found.");

            await using var client = new ServiceBusClient(connectionString);
            await using var sender = client.CreateSender(queueName);

            var message = new ServiceBusMessage(JsonSerializer.Serialize(mailRequest))
            {
                ContentType = "application/json",
                Subject = mailRequest.Subject
            };

            await sender.SendMessageAsync(message);
            _logger.LogInformation("Mail request sent to queue for recipient: {Recipient}", mailRequest.RecipientAddress);

            return Ok(new { message = "Mail request has been queued" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending mail request to queue");
            return StatusCode(500, new { message = "Error processing mail request" });
        }
    }
} 