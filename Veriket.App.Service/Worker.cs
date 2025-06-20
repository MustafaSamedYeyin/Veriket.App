
using Veriket.App.Core.Entities;
using Veriket.App.Core.Service;

namespace Veriket.App.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    private LogService _logService;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        var logFolderName = _configuration["VeriketLogSettings:LogFolder"];
        var logFileName = _configuration["VeriketLogSettings:LogFileName"];
        _logService = new LogService(logFolderName,logFileName);
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
