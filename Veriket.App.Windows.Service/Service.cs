using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veriket.App.Shared.Entities;
using Veriket.App.Shared.Service;

namespace Veriket.App.Windows.Service
{
    partial class Service : ServiceBase
    {
        private LogService _logService;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _executingTask;

        public Service()
        {
            InitializeComponent();
            ServiceName = "VeriketAppService";
            var logFolderName = ConfigurationManager.AppSettings["VeriketLogSettings:LogFolder"];
            var logFileName = ConfigurationManager.AppSettings["VeriketLogSettings:LogFileName"];
            _logService = new LogService(logFolderName, logFileName);
        }

        protected override void OnStart(string[] args)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _executingTask = ExecuteAsync(_cancellationTokenSource.Token);
        }

        protected override void OnStop()
        {
            _cancellationTokenSource.Cancel();
            _executingTask.Wait();
        }

        private async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var log = new Log
            {
                ComputerName = Environment.MachineName,
                User = Environment.UserName
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logService.WriteLog(log);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(ServiceName, ex.ToString(), EventLogEntryType.Error);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
