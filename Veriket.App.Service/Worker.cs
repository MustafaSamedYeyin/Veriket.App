
using Veriket.App.Core.Entities;
using Veriket.App.Core.Service;

namespace Veriket.App.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private LogService _logService;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _logService = new LogService();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var log = new Log
        {
            ComputerName = Environment.MachineName,
            User = Environment.UserName
        };
        while (!stoppingToken.IsCancellationRequested)
        {
            _logService.WriteLog(log);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
