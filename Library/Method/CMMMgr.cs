using CMM.Library.Base;
using CMM.Library.ViewModel;

namespace CMM.Library.Method;

public class CMMMgr : PropertyBase
{
    public ObservableRangeCollection<XMonitor> Monitors
    {
        get => _Monitors;
        set { SetProperty(ref _Monitors, value); }
    }
    ObservableRangeCollection<XMonitor> _Monitors = new ();

    public async Task Init()
    {
        await CMMCommand.ScanMonitor();
        var monColle = new ObservableRangeCollection<XMonitor>();
        monColle.AddRange(await CMMCommand.ReadMonitorsData());
        Monitors = monColle;
        await CMMCommand.ScanMonitorStatus(monColle);
    }
}
