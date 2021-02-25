using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoApi.Utils;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi.Services
{
    public class ReminderService : IHostedService, IDisposable
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ReminderOptions _reminderOptions;
        private readonly ILogger<ReminderService> _logger;
        private Timer _timer;

        public ReminderService(IServiceScopeFactory scopeFactory, IOptions<ReminderOptions> reminderOptions, ILogger<ReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _reminderOptions = reminderOptions.Value;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("ReminderService started!");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_reminderOptions.Interval));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogDebug("ReminderService.DoWork() executed!");
            using (var scope = _scopeFactory.CreateScope())
            {
                var toDoDbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
                var toDoLists = toDoDbContext.ToDoLists.Where(x => !x.IsReminded && DateTime.Now > x.ReminderDateTime).ToList();
                _logger.LogDebug("ReminderService found {0} ToDoLists!", toDoLists.Count);

                foreach (ToDoList toDoList in toDoLists)
                {
                    SendEmail(_reminderOptions.ToDoListLink + toDoList.Id, toDoList.Owner);
                    _logger.LogDebug("ReminderService.SendEmail() executed!");
                    toDoList.IsReminded = true;
                }
                toDoDbContext.SaveChanges();
               
            }
        }

        private void SendEmail(string text, string userEmail)
        {
            var apiKey = _reminderOptions.SendGridKey;
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage()
            {
                From = new EmailAddress(_reminderOptions.EmailFrom, _reminderOptions.NameFrom),
                Subject = _reminderOptions.Subject,
                PlainTextContent = text,
            };
            message.AddTo(new EmailAddress(userEmail, _reminderOptions.NameTo));
            client.SendEmailAsync(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("ReminderService stopped!");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
