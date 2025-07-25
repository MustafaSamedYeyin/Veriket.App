﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veriket.App.Shared.Entities;

namespace Veriket.App.Shared.Service;
public class LogService
{
    private readonly string _logFilePath;
    public LogService(string logFolderName, string logFileName)
    {
        _logFilePath = GetLogFileLocation(logFolderName,logFileName);
    }

    public void WriteLog(Log log)
    {
        using (StreamWriter writer = File.AppendText(_logFilePath))
        {
            writer.WriteLine($"{DateTime.Now.ToString(GetLogDateTimeFormat())},{log.ComputerName},{log.User}");
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
    private string GetLogFileLocation(string logFolderName,string logFileName)
    {
        var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        var filePath =  Path.Combine(programDataPath, logFolderName, logFileName);

        EnsureDirectoryExist(filePath);

        return filePath;
    }

    public string GetLogDateTimeFormat()
    {
        return "yyyy-MM-dd HH:mm:ss";
    }
    #endregion
}