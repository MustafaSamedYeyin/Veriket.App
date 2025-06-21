using System;

namespace Veriket.App.Shared.Entities;

public class Log
{
    public DateTime Date { get; set; }
    public string ComputerName { get; set; } = Environment.MachineName;
    public string User { get; set; } = Environment.UserName;
}