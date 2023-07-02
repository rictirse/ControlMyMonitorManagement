using CMM.Library.Base;
using CMM.Library.Helpers;
using CMM.Library.ViewModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace CMM.Library.Method;

/// <summary> 
/// Control My Monitor Management Command
/// </summary>
public static class CMMCommand
{
    static readonly string CMMTmpFolder = Path.Combine(Path.GetTempPath(), $"CMM");
    static readonly string CMMexe       = Path.Combine(CMMTmpFolder, "ControlMyMonitor.exe");
    static readonly string CMMsMonitors = Path.Combine(CMMTmpFolder, "smonitors.tmp");

    public static async Task ScanMonitor()
    {
        await BytesToFileAsync(new(CMMexe));
        await ConsoleHelper.CmdCommandAsync($"{CMMexe} /smonitors {CMMsMonitors}");
    }

    public static Task PowerOn(string monitorSN)
    { 
        return ConsoleHelper.CmdCommandAsync($"{CMMexe} /SetValue {monitorSN} D6 1");
    }

    public static Task Sleep(string monitorSN)
    {
        return ConsoleHelper.CmdCommandAsync($"{CMMexe} /SetValue {monitorSN} D6 4");
    }

    private static async Task<string> GetMonitorValue(string monitorSN)
    {
        var cmdFileName = Path.Combine(CMMTmpFolder, $"{Guid.NewGuid()}.bat");
        var cmd = $"{CMMexe} /GetValue {monitorSN} D6\r\n" +
                  $"echo %errorlevel%";
        File.WriteAllText(cmdFileName, cmd);
        var values = await ConsoleHelper.ExecuteCommand(cmdFileName);
        File.Delete(cmdFileName);
        return values.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
    }

    public static async Task<string> GetMonPowerStatus(string monitorSN)
    {
        var status = await GetMonitorValue(monitorSN);

        return status switch
        {
            "1" => "PowerOn",
            "4" => "Sleep",
            "5" => "PowerOff",
            _ => string.Empty
        };
    }

    public static async Task ScanMonitorStatus(IEnumerable<XMonitor> monitors)
    {
        var taskList = monitors.Select(x =>
        {
            return ScanMonitorStatus($"{CMMTmpFolder}\\{x.SerialNumber}.tmp", x);
        });

        await Task.WhenAll(taskList);
    }

    static async Task ScanMonitorStatus(string savePath, XMonitor mon)
    {
        await ConsoleHelper.CmdCommandAsync($"{CMMexe} /sjson {savePath} {mon.MonitorID}");
        var monitorModel = JsonHelper.JsonFormFile<IEnumerable<SMonitorModel>>(savePath);

        var status = monitorModel.ReadMonitorStatus();

        mon.Status = new ObservableRangeCollection<XMonitorStatus>(status);
    }

    /// <summary>
    /// 取得螢幕狀態
    /// </summary>
    public static IEnumerable<XMonitorStatus> ReadMonitorStatus(this IEnumerable<SMonitorModel> monitorModel)
    {
        foreach (var m in monitorModel)
        {       
            yield return new XMonitorStatus
            {
                VCP_Code = m.VCPCode,
                VCPCodeName = m.VCPCodeName,
                Read_Write = m.ReadWrite,
                CurrentValue = TryGetInt(m.CurrentValue),
                MaximumValue = TryGetInt(m.MaximumValue),
                PossibleValues = TryGetArrStr(m.PossibleValues),
            };
        }

        IEnumerable<int> TryGetArrStr(string str)
        {
            return str.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => TryGetInt(x))
                .Where(x => x != null)
                .Select(x => (int)x)
                .ToList();
        }

        int? TryGetInt(string str)
        {
            return int.TryParse(str, out var value)
                ? value
                : null;
        }
    }

    /// <summary>
    /// 取得螢幕清單
    /// </summary>
    public static async Task<IEnumerable<XMonitor>> ReadMonitorsData()
    {
        var monitors = new List<XMonitor>();

        if (!File.Exists(CMMsMonitors)) return monitors;

        XMonitor mon = null;
        string context;
        foreach (var line in await File.ReadAllLinesAsync(CMMsMonitors))
        {
            var sp = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (sp.Length != 2 || string.IsNullOrEmpty(sp[1])) continue;

                context = sp[1].Substring(2, sp[1].Length - 3);
            }
            catch
            {
                continue;
            }

            if (sp[0].StartsWith("Monitor Device Name"))
            {
                mon = new XMonitor();
                mon.MonitorDeviceName = context;
                continue;
            }

            if (sp[0].StartsWith("Monitor Name"))
            {
                mon.MonitorName = context;
                continue;
            }

            if (sp[0].StartsWith("Serial Number"))
            {
                mon.SerialNumber = context;
                continue;
            }

            if (sp[0].StartsWith("Adapter Name"))
            {
                mon.AdapterName = context;
                continue;
            }

            if (sp[0].StartsWith("Monitor ID"))
            {
                mon.MonitorID = context;
                monitors.Add(mon);
                continue;
            }
        }

        return monitors;
    }

    static void BytesToFile(FileInfo fi)
    {
        fi.Refresh();
        if (fi.Exists) return;
        if (!fi.Directory.Exists) fi.Directory.Create();

        File.WriteAllBytes(fi.FullName, fi.Name.ResourceToByteArray());
    }

    static async Task BytesToFileAsync(FileInfo fi)
    {
        fi.Refresh();
        if (fi.Exists) return;
        if (!fi.Directory.Exists) fi.Directory.Create();

        await File.WriteAllBytesAsync(fi.FullName, fi.Name.ResourceToByteArray());
    }

}
