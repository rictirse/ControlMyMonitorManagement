using CMM.Library.Base;
using CMM.Library.Helpers;
using CMM.Library.Method;
using CMM.Library.ViewModel;

namespace CMM.Tester;

public class CommnadTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        await CMMCommand.ScanMonitor();
        var monColle = new ObservableRangeCollection<XMonitor>();
        monColle.AddRange(await CMMCommand.ReadMonitorsData());
        await CMMCommand.ScanMonitorStatus(monColle);
    }

    [Test]
    public void JsonParserTest()
    {
        var path = @"C:\Users\shoop\AppData\Local\Temp\CMM\KV97067ICLCL.tmp";

        var monitorModel = JsonHelper.JsonFormFile<IEnumerable<SMonitorModel>>(path);

    }

    [Test]
    public async Task GetMonPowerStatus()
    {
        var status = await CMMCommand.GetMonPowerStatus("CBBP3P3");
    }
}