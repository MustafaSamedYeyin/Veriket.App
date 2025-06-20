using Veriket.App.Core.Service;

namespace Veriket.App.Viewer;

public partial class Form1 : Form
{
    public LogService _logService;
    public Form1()
    {
        _logService = new LogService();
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var logs = _logService.ReadLogs();

        dataGridView1.DataSource = logs.Select(log => new
        {
            Tarih = log.Date.ToString("yyyy-MM-dd HH:mm:ss"),
            BilgisayarAdı = log.ComputerName,
            KullanıcıAdi = log.User
        }).ToList();
    }
}
