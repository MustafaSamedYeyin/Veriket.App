using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veriket.App.Core.Entities;

namespace Veriket.App.Core.Service;
public class LogService
{
    private readonly string _logFilePath;
    public LogService()
    {
        _logFilePath = GetLogFileLocation();
    }

    public void WriteLog(Log log)
    {
        using (StreamWriter writer = new StreamWriter(_logFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{log.ComputerName},{log.User}");
        }
    }

    public List<Log> ReadLogs()
    {
        List<Log> logs = new List<Log>();

        try
        {
            using (StreamReader reader = new StreamReader(_logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            logs.Add(new Log
                            {
                                Date = DateTime.Parse(parts[0]),
                                ComputerName = parts[1],
                                User = parts[2]
                            });
                        }
                    }
                }
            }
        }
        catch (IOException ex)
        {
            // Consider logging or handling the exception appropriately
            Console.WriteLine($"Error reading log file: {ex.Message}");
        }

        return logs;
    }

    private void EnsureDirectoryExist(string filePath)
    {
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    #region Private
    private string GetLogFileLocation()
    {
        var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var logFolderName = "VeriketApp";
        var logFileName = "VeriketAppTest.txt";

        var filePath =  Path.Combine(programDataPath, logFolderName, logFileName);

        EnsureDirectoryExist(filePath);

        return filePath;
    }
    #endregion
}