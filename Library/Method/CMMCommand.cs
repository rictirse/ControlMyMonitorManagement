using CMM.Library.Base;
using CMM.Library.Helpers;
using CMM.Library.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Library.Method
{
    /// <summary> 
    /// Control My Monitor Management Command
    /// </summary>
    public static class CMMCommand
    {
        static readonly string CMMTmpFolder = Path.Combine(Path.GetTempPath(), $"CMM");
        static readonly string CMMexe       = Path.Combine(CMMTmpFolder, "ControlMyMonitor.exe");
        static readonly string CMMsMonitors = Path.Combine(CMMTmpFolder, "smonitors.tmp");
        static readonly string CMMcfg       = Path.Combine(CMMTmpFolder, "ControlMyMonitor.cfg");

        public static async Task ScanMonitor()
        {
            await BytesToFileAsync(new(CMMexe));
            await ConsoleHelper.CmdCommandAsync($"{CMMexe} /smonitors {CMMsMonitors}");
        }

        public static async Task ScanMonitorInterfaces(IEnumerable<XMonitor> monitors)
        {
            var taskList = new List<Task>();
            foreach (var mon in monitors)
            {
                taskList.Add(Task.Run(async () => await
                    ScanMonitorInterfaces($"{CMMTmpFolder}\\{mon.SerialNumber}.tmp", mon)));
            }

            await Task.WhenAll(taskList.ToArray());
        }

        static async Task ScanMonitorInterfaces(string savePath, XMonitor mon)
        {
            //await ConsoleHelper.CmdCommandAsync($"{CMMexe} /scomma {savePath} {mon.MonitorID}");
            await mon.ReadMonitorStatus(savePath);
        }

        /// <summary>
        /// 取得螢幕狀態
        /// </summary>
        public static async Task ReadMonitorStatus(this XMonitor @this, string filePath)
        {
            var statusColle = new ObservableRangeCollection<XMonitorStatus>();

            if (!File.Exists(filePath)) return;

            foreach (var line in await File.ReadAllLinesAsync(filePath))
            {
                var sp = line.Split(",");
                if (sp.Length < 6) continue;
                
                statusColle.Add(new XMonitorStatus
                {
                    VCP_Code       = StrTrim(sp[0]),
                    VCPCodeName    = StrTrim(sp[1]),
                    Read_Write     = StrTrim(sp[2]),
                    CurrentValue   = TryGetInt(sp[3]),
                    MaximumValue   = TryGetInt(sp[4]),
                    PossibleValues = TryGetArrStr(sp),
                });
            }

            @this.Status = statusColle;

            string StrTrim(string str)
            {
                if (string.IsNullOrEmpty(str)) return null;
                return str;
            }

            string TryGetArrStr(string[] strArr)
            {
                if (strArr.Length < 7) return null;

                var outStr = string.Join(",", strArr[5..]);
                outStr = outStr.Substring(1, outStr.Length - 2);
                return outStr;
            }

            int? TryGetInt(string str)
            {
                if (int.TryParse(str, out var value)) return value;
                return null;
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

}
